import { Link } from 'react-router-dom'

export function PageFrame({ children, chip = 'BETA' }) {
  return (
    <main className="page-shell">
      <div className="bg-grid" />
      <div className="bg-glow left" />
      <div className="bg-glow right" />

      <section className="card">
        <div className="card-inner">
          <header className="top-nav">
            <Link to="/" className="brand">
              JILO
            </Link>
            <span className="chip">{chip}</span>
          </header>
          {children}
        </div>
      </section>
    </main>
  )
}
