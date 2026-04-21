import { Link } from 'react-router-dom'
import { useAuth } from '../context/useAuth'
import { InvitationsPopover } from './InvitationsPopover'

export function AuthenticatedLayout({ avatarUrl, username, children }) {
  const { isAuthenticated } = useAuth()

  return (
    <main className="dashboard-shell">
      <div className="bg-grid" />
      <div className="bg-glow left" />
      <div className="bg-glow right" />

      <header className="dashboard-topbar">
        <Link to="/" className="brand">
          JILO
        </Link>

        <div className="topbar-right">
          {isAuthenticated ? (
            <>
              <InvitationsPopover />
              <Link className="avatar-link" to="/profile" title="Личный кабинет">
                {avatarUrl ? (
                  <img className="avatar-img" src={avatarUrl} alt={username || 'avatar'} />
                ) : (
                  <span className="avatar-fallback">
                    {(username || 'U').slice(0, 1).toUpperCase()}
                  </span>
                )}
              </Link>
            </>
          ) : (
            <Link className="button secondary" to="/login">
              Вход
            </Link>
          )}
        </div>
      </header>

      {children}
    </main>
  )
}
