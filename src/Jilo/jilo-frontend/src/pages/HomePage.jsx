import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { AuthenticatedLayout } from '../components/AuthenticatedLayout'
import { getMyProfile } from '../api/profile'
import { useAuth } from '../context/useAuth'

const newsItems = [
  {
    title: 'Новый сезон в шутерах',
    text: 'Команды активно ищут саппортов и снайперов под рейтинговые матчи.',
  },
  {
    title: 'Турнирные сквады недели',
    text: 'Подборка коллективов, которым нужны игроки на постоянную основу.',
  },
  {
    title: 'Советы по синергии ролей',
    text: 'Как быстрее найти баланс между капитаном, керри и саппортом.',
  },
]

const teamPosts = [
  { team: 'Night Owls', game: 'Valorant', need: 'Duelist', rank: 'Diamond+' },
  { team: 'Arcadia Core', game: 'Dota 2', need: 'Support 4/5', rank: 'Ancient+' },
  { team: 'Zero Gravity', game: 'CS2', need: 'IGL', rank: 'Level 8 FACEIT+' },
]

export function HomePage() {
  const { isAuthenticated } = useAuth()
  const [profile, setProfile] = useState(null)

  useEffect(() => {
    const loadProfile = async () => {
      if (!isAuthenticated) {
        setProfile(null)
        return
      }

      try {
        const data = await getMyProfile()
        setProfile(data)
      } catch {
        setProfile(null)
      }
    }

    loadProfile()
  }, [isAuthenticated])

  return (
    <AuthenticatedLayout avatarUrl={profile?.avatarUrl} username={profile?.username}>
      <div className="dashboard-grid">
        <aside className="dashboard-sidebar">
          <h3>Меню</h3>
          <nav className="dashboard-nav">
            <Link to="/">Главная</Link>
            {isAuthenticated ? (
              <>
                <Link to="/profile">Личный кабинет</Link>
                <Link to="/lobby">Лобби</Link>
              </>
            ) : (
              <Link to="/login">Войти в аккаунт</Link>
            )}
             <a>Новости</a>
             <a>Поиск команд</a>
          </nav>
        </aside>

        <section className="dashboard-feed">
          <h1>Jilo — платформа для поиска тиммейтов в онлайн-играх по ролям, рангу и стилю игры.</h1>
          <p className="hero-subtitle">
            Здесь ты можешь быстро собрать команду, найти игроков под свой уровень и отслеживать актуальные наборы в составы.
          </p>

          <div id="news" className="feed-block">
            <h2>Новости</h2>
            <div className="feed-list">
              {newsItems.map((news) => (
                <article className="feed-item" key={news.title}>
                  <h4>{news.title}</h4>
                  <p>{news.text}</p>
                </article>
              ))}
            </div>
          </div>

          <div id="teams" className="feed-block">
            <h2>Команды ищут тиммейтов</h2>
            <div className="feed-list">
              {teamPosts.map((post) => (
                <article className="feed-item" key={post.team}>
                  <h4>{post.team}</h4>
                  <p>
                    {post.game} · Нужна роль: {post.need}
                  </p>
                  <p className="feed-meta">Требуемый ранг: {post.rank}</p>
                </article>
              ))}
            </div>
          </div>
        </section>
      </div>
    </AuthenticatedLayout>
  )
}
