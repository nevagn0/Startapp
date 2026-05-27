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
    const [cookieConsent, setCookieConsent] = useState(true)

    useEffect(() => {
        const consent = localStorage.getItem('cookie_consent')
        if (!consent) {
            setCookieConsent(false)
        }
    }, [])

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

    const handleCookieAccept = () => {
        localStorage.setItem('cookie_consent', 'true')
        setCookieConsent(true)
    }

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

            {!cookieConsent && (
                <div
                    style={{
                        position: 'fixed',
                        bottom: 0,
                        left: 0,
                        right: 0,
                        zIndex: 1000,
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        padding: '16px 24px',
                        // Полупрозрачная подложка на весь баннер
                        background: 'rgba(0, 0, 0, 0.75)',
                        backdropFilter: 'blur(8px)',
                        borderTop: '2px solid var(--accent)',
                        flexWrap: 'wrap',
                        textAlign: 'center',
                    }}
                >
                    <div
                        style={{
                            maxWidth: '650px',
                            background: 'var(--bg)',      // тёмный/светлый фон вашей темы
                            color: 'var(--text)',
                            padding: '12px 20px',
                            borderRadius: '12px',
                            display: 'flex',
                            flexWrap: 'wrap',
                            alignItems: 'center',
                            justifyContent: 'center',
                            gap: '12px',
                            boxShadow: '0 8px 24px rgba(0,0,0,0.4)',
                        }}
                    >
                        <span style={{ fontSize: '14px', lineHeight: 1.5 }}>
                            Мы используем только технически необходимые данные (токены аутентификации) для работы
                            сайта и не применяем отслеживающие или маркетинговые cookie.
                            Ознакомьтесь с нашей{' '}
                            <Link
                                to="/policy"
                                style={{
                                    color: 'var(--accent)',
                                    textDecoration: 'underline',
                                    fontWeight: 500,
                                }}
                            >
                                Политикой обработки данных
                            </Link>.
                        </span>
                        <button
                            className="button primary"
                            onClick={handleCookieAccept}
                            style={{ whiteSpace: 'nowrap', padding: '8px 20px' }}
                        >
                            Понятно
                        </button>
                    </div>
                </div>
            )}
        </AuthenticatedLayout>
    )
}