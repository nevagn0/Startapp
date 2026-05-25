import { Link } from 'react-router-dom'
import { PageFrame } from '../components/PageFrame'

export function MainPage() {
  return (
    <PageFrame chip="MATCHMAKING">
      <div className="hero-layout">
        <div>
          <h1 className="hero-title">
            Найди идеальных <span>тиммейтов</span> за пару минут
          </h1>
          <p className="hero-subtitle">
            Jilo помогает собирать стабильные игровые команды по рейтингу, роли,
            стилю игры и любимым дисциплинам. Никакого хаоса в чатах - только
            точный подбор союзников.
          </p>

          <div className="feature-list">
            <span className="feature-item">Рейтинг игроков</span>
            <span className="feature-item">Фильтры по стилю игры</span>
            <span className="feature-item">Поиск по ролям</span>
            <span className="feature-item">Система доверия</span>
          </div>

          <div style={{ marginTop: '1.4rem', display: 'flex', gap: '0.65rem', flexWrap: 'wrap' }}>
            <Link className="button primary" to="/register">
              Создать аккаунт
            </Link>
            <Link className="button secondary" to="/login">
              Войти в систему
            </Link>
          </div>
        </div>

        <aside className="stat-grid">
          <div className="stat-box">
            <p className="stat-value">25 000+</p>
            <p className="stat-label">игроков уже в экосистеме Jilo</p>
          </div>
          <div className="stat-box">
            <p className="stat-value">1.8 мин</p>
            <p className="stat-label">среднее время подбора команды</p>
          </div>
          <div className="stat-box">
            <p className="stat-value">4.9 / 5</p>
            <p className="stat-label">оценка качества матчей с новыми тиммейтами</p>
          </div>
        </aside>
      </div>
    </PageFrame>
  )
}
