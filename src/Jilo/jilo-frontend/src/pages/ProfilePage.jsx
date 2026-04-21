import { useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { AuthenticatedLayout } from '../components/AuthenticatedLayout'
import {
  addMyGame,
  deleteMyGame,
  getAvailableGames,
  getMyGames,
  getMyProfile,
  updateMyGame,
  updateMyBio,
  uploadMyAvatar,
} from '../api/profile'
import { useAuth } from '../context/useAuth'

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

export function ProfilePage() {
  const navigate = useNavigate()
  const { signOut } = useAuth()
  const [profile, setProfile] = useState(null)
  const [myGames, setMyGames] = useState([])
  const [allGames, setAllGames] = useState([])
  const [bio, setBio] = useState('')
  const toastTimerRef = useRef(null)
  const [toast, setToast] = useState(null)
  const [isBusy, setIsBusy] = useState(false)
  const [isBioEditing, setIsBioEditing] = useState(false)
  const [editingGameId, setEditingGameId] = useState('')
  const [editingGameForm, setEditingGameForm] = useState({ role: '', rank: '' })
  const [gameForm, setGameForm] = useState({ gameId: '', role: '', rank: '' })

  const selectedGame = useMemo(
    () => allGames.find((game) => game.id === gameForm.gameId),
    [allGames, gameForm.gameId],
  )

  const roleRankOptions = useMemo(() => {
    const key = normalizeGameKey(selectedGame?.gameName || selectedGame?.name)
    return presets[key] || presets.default
  }, [selectedGame])

  const editingGame = useMemo(
    () => myGames.find((item) => item.id === editingGameId),
    [editingGameId, myGames],
  )

  const editRoleRankOptions = useMemo(() => {
    const key = normalizeGameKey(editingGame?.gameName)
    return presets[key] || presets.default
  }, [editingGame])

  const reload = async () => {
    const [profileData, gamesData, myGamesData] = await Promise.all([
      getMyProfile(),
      getAvailableGames(),
      getMyGames(),
    ])
    setProfile(profileData)
    setBio(profileData?.bio || '')
    setAllGames(gamesData || [])
    const normalizedMyGames = Array.isArray(myGamesData)
      ? myGamesData
      : myGamesData?.games || myGamesData?.items || []
    setMyGames(normalizedMyGames)
  }

  const showToast = (message, type = 'success') => {
    if (toastTimerRef.current) {
      window.clearTimeout(toastTimerRef.current)
    }

    setToast({ message, type })
    toastTimerRef.current = window.setTimeout(() => {
      setToast(null)
    }, 2500)
  }

  useEffect(() => {
    const load = async () => {
      try {
        await reload()
      } catch (loadError) {
        showToast(loadError.message || 'Не удалось загрузить личный кабинет.', 'error')
      }
    }

    load()

    return () => {
      if (toastTimerRef.current) {
        window.clearTimeout(toastTimerRef.current)
      }
    }
  }, [])

  const onAvatarChange = async (event) => {
    const file = event.target.files?.[0]
    if (!file) return

    try {
      setIsBusy(true)
      await uploadMyAvatar(file)
      await reload()
      showToast('Аватар успешно обновлён.')
    } catch (uploadError) {
      showToast(uploadError.message || 'Не удалось загрузить аватар.', 'error')
    } finally {
      setIsBusy(false)
    }
  }

  const onToggleBioEdit = async () => {
    if (isBioEditing) {
      try {
        setIsBusy(true)
        await updateMyBio(bio)
        await reload()
        showToast('Блок "О себе" сохранён.')
      } catch (bioError) {
        showToast(bioError.message || 'Не удалось обновить профиль.', 'error')
        return
      } finally {
        setIsBusy(false)
      }
    }

    setIsBioEditing((prev) => !prev)
  }

  const onAddGame = async (event) => {
    event.preventDefault()

    try {
      setIsBusy(true)
      if (!gameForm.gameId || !gameForm.role || !gameForm.rank) {
        showToast('Для добавления игры выберите игру, роль и ранг.', 'error')
        return
      }

      await addMyGame({
        gameId: gameForm.gameId,
        role: gameForm.role,
        rank: gameForm.rank,
      })
      setGameForm({ gameId: '', role: '', rank: '' })
      await reload()
      showToast('Игра добавлена в ваш профиль.')
    } catch (addGameError) {
      showToast(addGameError.message || 'Не удалось добавить игру.', 'error')
    } finally {
      setIsBusy(false)
    }
  }

  const startEditingGame = (item) => {
    setEditingGameId(item.id)
    setEditingGameForm({
      role: item.role,
      rank: item.rank,
    })
  }

  const onUpdateGame = async (event) => {
    event.preventDefault()

    if (!editingGameId) {
      return
    }

    try {
      setIsBusy(true)
      await updateMyGame(editingGameId, {
        role: editingGameForm.role,
        rank: editingGameForm.rank,
      })
      setEditingGameId('')
      await reload()
      showToast('Параметры игры обновлены.')
    } catch (updateGameError) {
      showToast(updateGameError.message || 'Не удалось обновить игру.', 'error')
    } finally {
      setIsBusy(false)
    }
  }

  const onDeleteGame = async (userGameId) => {
    try {
      setIsBusy(true)
      await deleteMyGame(userGameId)
      await reload()
      showToast('Игра удалена из профиля.')
    } catch (deleteGameError) {
      showToast(deleteGameError.message || 'Не удалось удалить игру.', 'error')
    } finally {
      setIsBusy(false)
    }
  }

  const onLogout = async () => {
    const shouldLogout = window.confirm('Выйти из аккаунта?')
    if (!shouldLogout) {
      return
    }

    await signOut()
    navigate('/login')
  }

  return (
    <AuthenticatedLayout avatarUrl={profile?.avatarUrl} username={profile?.username}>
      {toast ? (
        <div className={`profile-toast ${toast.type === 'error' ? 'error' : 'success'}`}>
          {toast.message}
        </div>
      ) : null}

      <section className="profile-card">
        <h1>Личный кабинет</h1>

        <div className="profile-head">
          <div className="profile-avatar">
            {profile?.avatarUrl ? (
              <img src={profile.avatarUrl} alt={profile.username || 'avatar'} />
            ) : (
              <span>{(profile?.username || 'U').slice(0, 1).toUpperCase()}</span>
            )}
          </div>
          <div>
            <p className="profile-nick">{profile?.username || 'Игрок'}</p>
            <label className="button secondary file-button">
              Сменить аватар
              <input type="file" accept=".jpg,.jpeg,.png,.webp" onChange={onAvatarChange} />
            </label>
          </div>
        </div>

        <section className="profile-section">
          <div className="profile-section-header">
            <h3>О себе</h3>
            <button
              className="icon-button"
              type="button"
              onClick={onToggleBioEdit}
              disabled={isBusy}
              title={isBioEditing ? 'Сохранить' : 'Изменить'}
            >
              {isBioEditing ? '✓' : '✎'}
            </button>
          </div>
          <textarea
            className="input"
            rows={4}
            value={bio}
            onChange={(event) => setBio(event.target.value)}
            placeholder="Расскажите о вашем стиле игры и целях..."
            disabled={!isBioEditing}
          />
        </section>

        <form className="profile-section" onSubmit={onAddGame}>
          <h3>Добавить игру</h3>
          <div className="game-form-grid">
            <select
              className="input"
              value={gameForm.gameId}
              onChange={(event) =>
                setGameForm({ gameId: event.target.value, role: '', rank: '' })
              }
            >
              <option value="">Выберите игру</option>
              {allGames.map((game) => (
                <option key={game.id} value={game.id}>
                  {game.gameName || game.name}
                </option>
              ))}
            </select>

            <select
              className="input"
              value={gameForm.rank}
              onChange={(event) => setGameForm((prev) => ({ ...prev, rank: event.target.value }))}
              disabled={!gameForm.gameId}
            >
              <option value="">Выберите ранг</option>
              {roleRankOptions.ranks.map((rank) => (
                <option key={rank} value={rank}>
                  {rank}
                </option>
              ))}
            </select>

            <select
              className="input"
              value={gameForm.role}
              onChange={(event) => setGameForm((prev) => ({ ...prev, role: event.target.value }))}
              disabled={!gameForm.gameId}
            >
              <option value="">Выберите роль</option>
              {roleRankOptions.roles.map((role) => (
                <option key={role} value={role}>
                  {role}
                </option>
              ))}
            </select>
          </div>
          <button className="button primary" type="submit" disabled={isBusy}>
            Добавить игру
          </button>
        </form>

        <section className="profile-section">
          <h3>Мои игры</h3>
          <div className="feed-list">
            {myGames.length === 0 ? (
              <p className="hero-subtitle">Пока нет добавленных игр.</p>
            ) : (
              myGames.map((item) => (
                <article className="feed-item" key={item.id}>
                  <h4>{item.gameName}</h4>
                  {editingGameId === item.id ? (
                    <form className="game-edit-grid" onSubmit={onUpdateGame}>
                      <select
                        className="input"
                        value={editingGameForm.rank}
                        onChange={(event) =>
                          setEditingGameForm((prev) => ({ ...prev, rank: event.target.value }))
                        }
                      >
                        {editRoleRankOptions.ranks.map((rank) => (
                          <option key={rank} value={rank}>
                            {rank}
                          </option>
                        ))}
                      </select>
                      <select
                        className="input"
                        value={editingGameForm.role}
                        onChange={(event) =>
                          setEditingGameForm((prev) => ({ ...prev, role: event.target.value }))
                        }
                      >
                        {editRoleRankOptions.roles.map((role) => (
                          <option key={role} value={role}>
                            {role}
                          </option>
                        ))}
                      </select>
                      <div className="game-card-actions">
                        <button className="button primary" type="submit" disabled={isBusy}>
                          Сохранить
                        </button>
                        <button
                          className="button secondary"
                          type="button"
                          onClick={() => setEditingGameId('')}
                          disabled={isBusy}
                        >
                          Отмена
                        </button>
                      </div>
                    </form>
                  ) : (
                    <>
                      <p>Роль: {item.role}</p>
                      <p className="feed-meta">Ранг: {item.rank}</p>
                      <div className="game-card-actions">
                        <button
                          className="button secondary"
                          type="button"
                          onClick={() => startEditingGame(item)}
                          disabled={isBusy}
                        >
                          Изменить
                        </button>
                        <button
                          className="button danger"
                          type="button"
                          onClick={() => onDeleteGame(item.id)}
                          disabled={isBusy}
                        >
                          Удалить
                        </button>
                      </div>
                    </>
                  )}
                </article>
              ))
            )}
          </div>
        </section>

        <section className="profile-section">
          <button className="button danger small-logout" type="button" onClick={onLogout}>
            Выйти из аккаунта
          </button>
        </section>
      </section>
    </AuthenticatedLayout>
  )
}
