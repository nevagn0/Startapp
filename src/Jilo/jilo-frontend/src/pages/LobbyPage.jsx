import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { AuthenticatedLayout } from '../components/AuthenticatedLayout'
import { getAvailableGames, getMyProfile } from '../api/profile'
import {
  createLobby,
  deleteLobby,
  getCurrentLobby,
  getOutgoingLobbyInvitations,
  kickFromLobby,
  leaveLobby,
  sendLobbyInvitation,
} from '../api/lobby'
import { searchPlayers } from '../api/playerSearch'

const TEAM_SIZE = 5
const OWNER_SLOT_INDEX = 2
const presets = {
  default: {
    roles: ['Игрок', 'Саппорт', 'Штурмовик', 'Снайпер'],
    ranks: ['Новичок', 'Средний', 'Продвинутый', 'Эксперт'],
  },
  valorant: {
    roles: ['Duelist', 'Initiator', 'Sentinel', 'Controller'],
    ranks: ['Iron', 'Bronze', 'Silver', 'Gold', 'Platinum', 'Diamond', 'Ascendant', 'Immortal', 'Radiant'],
  },
  'dota 2': {
    roles: ['Carry', 'Mid', 'Offlane', 'Soft Support', 'Hard Support'],
    ranks: ['Herald', 'Guardian', 'Crusader', 'Archon', 'Legend', 'Ancient', 'Divine', 'Immortal'],
  },
  'counter-strike 2': {
    roles: ['Entry Fragger', 'AWPer', 'Support', 'IGL', 'Lurker'],
    ranks: ['Silver', 'Gold Nova', 'MG', 'DMG', 'LE', 'LEM', 'SMFC', 'Global Elite'],
  },
}

const normalizeGameKey = (name) => (name || '').trim().toLowerCase()

export function LobbyPage() {
  const [profile, setProfile] = useState(null)
  const [games, setGames] = useState([])
  const [lobby, setLobby] = useState(null)
  const [outgoingInvitations, setOutgoingInvitations] = useState([])
  const [selectedGameId, setSelectedGameId] = useState('')
  const [isInviteModalOpen, setIsInviteModalOpen] = useState(false)
  const [inviteFilter, setInviteFilter] = useState({ role: '', rank: '' })
  const [playerResults, setPlayerResults] = useState([])
  const [isBusy, setIsBusy] = useState(false)
  const [error, setError] = useState('')
  const outgoingPollRef = useRef(null)

  const stopOutgoingPolling = useCallback(() => {
    if (outgoingPollRef.current) {
      window.clearInterval(outgoingPollRef.current)
      outgoingPollRef.current = null
    }
  }, [])

  const startOutgoingPolling = useCallback(
    (lobbyId) => {
      if (!lobbyId) return
      stopOutgoingPolling()
      outgoingPollRef.current = window.setInterval(async () => {
        try {
          const outgoing = await getOutgoingLobbyInvitations(lobbyId)
          setOutgoingInvitations(outgoing?.invitations || [])
        } catch {
          // ignore
        }
      }, 10000)
    },
    [stopOutgoingPolling],
  )

  function normalizeInvitationStatus(status) {
    const value = (status || '').toString().toLowerCase()
    if (value === 'declined' || value === 'expired') return 'Отклонено'
    if (value === 'accepted') return 'Принято'
    if (value === 'pending') return 'Ожидает ответа'
    return status || '—'
  }

  const members = useMemo(() => lobby?.members || [], [lobby])
  const lobbyGame = useMemo(
    () => games.find((game) => game.id === lobby?.gameId),
    [games, lobby?.gameId],
  )
  const filterOptions = useMemo(() => {
    const key = normalizeGameKey(lobbyGame?.gameName || lobbyGame?.name)
    return presets[key] || presets.default
  }, [lobbyGame])
  const currentUserId = profile?.id
  const owner = members.find((member) => member.isOwner) || members[0]
  const isCurrentUserOwner = Boolean(owner && owner.id === currentUserId)

  const loadLobbyData = useCallback(async () => {
    setError('')
    const [profileData, gamesData] = await Promise.all([getMyProfile(), getAvailableGames()])
    setProfile(profileData)
    setGames(gamesData || [])
    if (!selectedGameId && gamesData?.length) {
      setSelectedGameId(gamesData[0].id)
    }

    try {
      const currentLobby = await getCurrentLobby()
      setLobby(currentLobby)
      const lobbyOwner = currentLobby?.members?.find((m) => m.isOwner)
      const isOwnerAtLoad = Boolean(lobbyOwner && lobbyOwner.id === profileData?.id)

      if (isOwnerAtLoad) {
        const outgoing = await getOutgoingLobbyInvitations(currentLobby.id)
        setOutgoingInvitations(outgoing?.invitations || [])
        startOutgoingPolling(currentLobby.id)
      } else {
        setOutgoingInvitations([])
        stopOutgoingPolling()
      }
    } catch (lobbyError) {
      if (lobbyError?.status !== 404) {
        setError(lobbyError.message || 'Не удалось загрузить лобби.')
      }
      setLobby(null)
      setOutgoingInvitations([])
      stopOutgoingPolling()
    }
  }, [selectedGameId, startOutgoingPolling, stopOutgoingPolling])

  useEffect(() => {
    const timer = window.setTimeout(() => {
      loadLobbyData().catch(() => {})
    }, 0)

    return () => window.clearTimeout(timer)
  }, [loadLobbyData])

  useEffect(() => {
    return () => {
      stopOutgoingPolling()
    }
  }, [stopOutgoingPolling])

  const createNewLobby = async () => {
    if (!selectedGameId) return

    try {
      setIsBusy(true)
      const created = await createLobby(selectedGameId)
      const freshLobby = await getCurrentLobby()
      setLobby(freshLobby || created)
      setOutgoingInvitations([])
      setError('')
      startOutgoingPolling((freshLobby || created).id)
    } catch (createError) {
      setError(createError.message || 'Не удалось создать лобби.')
    } finally {
      setIsBusy(false)
    }
  }

  const leaveOrDeleteLobby = async () => {
    if (!lobby?.id) return

    try {
      setIsBusy(true)
      if (isCurrentUserOwner) {
        await deleteLobby(lobby.id)
      } else {
        await leaveLobby(lobby.id)
      }
      setLobby(null)
      setOutgoingInvitations([])
      stopOutgoingPolling()
    } catch (actionError) {
      setError(actionError.message || 'Не удалось выполнить действие с лобби.')
    } finally {
      setIsBusy(false)
    }
  }

  const findPlayers = async () => {
    if (!lobby?.gameId) return

    try {
      setIsBusy(true)
      const result = await searchPlayers({
        gameId: lobby.gameId,
        role: inviteFilter.role,
        rank: inviteFilter.rank,
      })

      const incoming = Array.isArray(result) ? result : result?.players || []
      const memberIds = new Set(members.map((item) => item.id))
      setPlayerResults(
        incoming.filter(
          (player) =>
            !memberIds.has(player.profileId) && player.profileId !== owner?.id && player.profileId !== profile?.id,
        ),
      )
      setError('')
    } catch (searchError) {
      setPlayerResults([])
      setError(
        searchError.message ||
          'Поиск игроков временно недоступен. Проверь backend endpoint /api/v1/search/players.',
      )
    } finally {
      setIsBusy(false)
    }
  }

  const sendInvite = async (playerId) => {
    if (!lobby?.id) return

    try {
      setIsBusy(true)
      await sendLobbyInvitation(lobby.id, playerId)
      const outgoing = await getOutgoingLobbyInvitations(lobby.id)
      setOutgoingInvitations(outgoing?.invitations || [])
    } catch (inviteError) {
      setError(inviteError.message || 'Не удалось отправить приглашение.')
    } finally {
      setIsBusy(false)
    }
  }

  const kickPlayer = async (profileId) => {
    if (!lobby?.id) return

    try {
      setIsBusy(true)
      await kickFromLobby(lobby.id, profileId)
      const freshLobby = await getCurrentLobby()
      setLobby(freshLobby)
    } catch (kickError) {
      setError(kickError.message || 'Не удалось удалить игрока из лобби.')
    } finally {
      setIsBusy(false)
    }
  }

  const memberSlots = useMemo(() => {
    const slots = Array.from({ length: TEAM_SIZE }, () => null)
    const ownerMember = members.find((member) => member.isOwner) || null
    const otherMembers = members.filter((member) => !member.isOwner)
    const fillOrder = [0, 1, 3, 4]

    slots[OWNER_SLOT_INDEX] = ownerMember
    fillOrder.forEach((slotIndex, idx) => {
      slots[slotIndex] = otherMembers[idx] || null
    })

    return slots
  }, [members])

  const renderSlot = (slotMember, index) => {
    if (index === OWNER_SLOT_INDEX) {
      return (
        <div className="lobby-slot owner" key={`slot-owner-${owner?.id || 'none'}`}>
          <p className="feed-meta">Лидер</p>
          <h3>{owner?.username || 'Пустой слот'}</h3>
          <p className="feed-meta">
            {owner ? `${owner.role} · ${owner.rank}` : 'Создай лобби'}
          </p>
        </div>
      )
    }

    if (!slotMember) {
      return (
        <button
          key={`slot-empty-${index}`}
          className="lobby-slot empty"
          type="button"
          onClick={openInviteModal}
          disabled={!isCurrentUserOwner}
          title={isCurrentUserOwner ? 'Пригласить игрока' : 'Только владелец приглашает'}
        >
          +
        </button>
      )
    }

    return (
      <div className="lobby-slot member" key={slotMember.id}>
        <h4>{slotMember.username}</h4>
        <p className="feed-meta">
          {slotMember.role} · {slotMember.rank}
        </p>
        {isCurrentUserOwner ? (
          <button className="button danger small-logout" type="button" onClick={() => kickPlayer(slotMember.id)}>
            Кик
          </button>
        ) : null}
      </div>
    )
  }

  const openInviteModal = async () => {
    setIsInviteModalOpen(true)
  }

  return (
    <AuthenticatedLayout avatarUrl={profile?.avatarUrl} username={profile?.username}>
      <section className="lobby-page">
        <div className="lobby-head">
          <h1>Лобби</h1>
        </div>

        {!lobby ? (
          <div className="lobby-create-card">
            <h3>Создать лобби</h3>
            <div className="lobby-create-controls">
              <select
                className="input"
                value={selectedGameId}
                onChange={(event) => setSelectedGameId(event.target.value)}
              >
                <option value="">Выберите игру</option>
                {games.map((game) => (
                  <option value={game.id} key={game.id}>
                    {game.gameName || game.name}
                  </option>
                ))}
              </select>
              <button className="button primary" type="button" onClick={createNewLobby} disabled={isBusy}>
                Создать лобби
              </button>
            </div>
          </div>
        ) : (
          <>
            <div className="lobby-arena">
              {memberSlots.map((slotMember, index) => renderSlot(slotMember, index))}
            </div>

            <div className="game-card-actions">
              {isCurrentUserOwner ?
              <button className="button secondary" type="button" onClick={openInviteModal} disabled={!isCurrentUserOwner}>
                Пригласить игрока
              </button> : null}
              <button className="button danger small-logout" type="button" onClick={leaveOrDeleteLobby}>
                {isCurrentUserOwner ? 'Распустить лобби' : 'Покинуть лобби'}
              </button>
            </div>

            <section className="profile-section">
              <h3>Исходящие приглашения</h3>
              <div className="feed-list">
                {outgoingInvitations.length === 0 ? (
                  <p className="hero-subtitle">Пока не отправлено ни одного приглашения.</p>
                ) : (
                  outgoingInvitations.map((item) => (
                    <article className="feed-item" key={item.id}>
                      <h4>{item.recieverUsername}</h4>
                      <p className="feed-meta">Статус: {normalizeInvitationStatus(item.status)}</p>
                    </article>
                  ))
                )}
              </div>
            </section>
          </>
        )}

        {isInviteModalOpen && lobby ? (
          <div className="overlay">
            <div className="modal-card">
              <div className="invite-popover-header">
                <h3>Пригласить игрока</h3>
                <button type="button" onClick={() => setIsInviteModalOpen(false)}>
                  Закрыть
                </button>
              </div>

              <div className="invite-filters">
                <select
                  className="input"
                  value={inviteFilter.role}
                  onChange={(event) => setInviteFilter((prev) => ({ ...prev, role: event.target.value }))}
                >
                  <option value="">Любая роль</option>
                  {filterOptions.roles.map((role) => (
                    <option value={role} key={role}>
                      {role}
                    </option>
                  ))}
                </select>
                <select
                  className="input"
                  value={inviteFilter.rank}
                  onChange={(event) => setInviteFilter((prev) => ({ ...prev, rank: event.target.value }))}
                >
                  <option value="">Любой ранг</option>
                  {filterOptions.ranks.map((rank) => (
                    <option value={rank} key={rank}>
                      {rank}
                    </option>
                  ))}
                </select>
                <button className="button primary" type="button" onClick={findPlayers} disabled={isBusy}>
                  Найти игроков
                </button>
              </div>

              <div className="feed-list">
                {playerResults.length === 0 ? (
                  <p className="hero-subtitle">Запусти поиск — покажем игроков по фильтрам роль/ранг.</p>
                ) : (
                  playerResults.map((player) => (
                    <article className="feed-item" key={`${player.profileId}-${player.addedAtUtc}`}>
                      <h4>{player.username}</h4>
                      <p>
                        {player.gameName} · {player.role} · {player.rank}
                      </p>
                      <button className="button secondary" type="button" onClick={() => sendInvite(player.profileId)}>
                        Пригласить
                      </button>
                    </article>
                  ))
                )}
              </div>
            </div>
          </div>
        ) : null}

        {error ? <div className="error-box">{error}</div> : null}
      </section>
    </AuthenticatedLayout>
  )
}
