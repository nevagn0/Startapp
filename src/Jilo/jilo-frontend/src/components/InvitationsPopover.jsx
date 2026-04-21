import { useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import {
  acceptInvitation,
  declineInvitation,
  getIncomingInvitations,
} from '../api/invitations'

const isExpired = (invitation) => {
  const expiresAt = invitation?.expiresAtUtc
  if (!expiresAt) return false
  return new Date(expiresAt).getTime() <= Date.now()
}

export function InvitationsPopover() {
  const navigate = useNavigate()
  const [isOpen, setIsOpen] = useState(false)
  const [invitations, setInvitations] = useState([])
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState('')
  const pruneTimerRef = useRef(null)
  const refreshTimerRef = useRef(null)

  const loadInvitations = async () => {
    try {
      setIsLoading(true)
      setError('')
      const data = await getIncomingInvitations()
      const incoming = data?.invitations || []
      setInvitations(incoming.filter((x) => !isExpired(x)))
    } catch (loadError) {
      setError(loadError.message || 'Не удалось загрузить приглашения.')
    } finally {
      setIsLoading(false)
    }
  }

  const stopTimers = () => {
    if (pruneTimerRef.current) {
      window.clearInterval(pruneTimerRef.current)
      pruneTimerRef.current = null
    }
    if (refreshTimerRef.current) {
      window.clearInterval(refreshTimerRef.current)
      refreshTimerRef.current = null
    }
  }

  const startTimers = async () => {
    stopTimers()

    pruneTimerRef.current = window.setInterval(() => {
      setInvitations((prev) => prev.filter((x) => !isExpired(x)))
    }, 1000)

    refreshTimerRef.current = window.setInterval(() => {
      loadInvitations().catch(() => {})
    }, 10000)
  }

  const togglePopover = async () => {
    const nextOpen = !isOpen
    setIsOpen(nextOpen)
    if (nextOpen) {
      await loadInvitations()
      await startTimers()
    } else {
      stopTimers()
    }
  }

  const onAccept = async (invitationId) => {
    try {
      await acceptInvitation(invitationId)
      setInvitations((prev) => prev.filter((item) => item.id !== invitationId))
      stopTimers()
      setIsOpen(false)
      navigate('/lobby')
    } catch (acceptError) {
      setError(acceptError.message || 'Не удалось принять приглашение.')
    }
  }

  const onDecline = async (invitationId) => {
    try {
      await declineInvitation(invitationId)
      setInvitations((prev) => prev.filter((item) => item.id !== invitationId))
    } catch (declineError) {
      setError(declineError.message || 'Не удалось отклонить приглашение.')
    }
  }

  return (
    <div className="invite-popover-wrap">
      <button className="button secondary" type="button" onClick={togglePopover}>
        Приглашения {invitations.length > 0 ? `(${invitations.length})` : ''}
      </button>

      {isOpen ? (
        <div className="invite-popover">
          <div className="invite-popover-header">
            <h4>Входящие приглашения</h4>
            <button type="button" onClick={loadInvitations}>
              Обновить
            </button>
          </div>

          {isLoading ? <p className="hero-subtitle">Загрузка...</p> : null}
          {error ? <p className="error-box">{error}</p> : null}

          {!isLoading && invitations.length === 0 ? (
            <p className="hero-subtitle">Приглашений пока нет.</p>
          ) : null}

          <div className="invite-list">
            {invitations.map((invitation) => (
              <article key={invitation.id} className="feed-item">
                <p>
                  <strong>{invitation.senderUsername}</strong> зовет в лобби
                </p>
                <p className="feed-meta">{invitation.gameName}</p>
                <div className="game-card-actions">
                  <button className="button primary" type="button" onClick={() => onAccept(invitation.id)}>
                    Принять
                  </button>
                  <button className="button danger" type="button" onClick={() => onDecline(invitation.id)}>
                    Отклонить
                  </button>
                </div>
              </article>
            ))}
          </div>
        </div>
      ) : null}
    </div>
  )
}
