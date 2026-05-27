import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { PageFrame } from '../components/PageFrame';

export function MainPage() {
    const [cookieConsent, setCookieConsent] = useState(true); // по умолчанию считаем, что согласие уже дано

    useEffect(() => {
        // Проверяем, есть ли запись о том, что пользователь уже видел уведомление
        const consent = localStorage.getItem('cookie_consent');
        if (!consent) {
            setCookieConsent(false); // показать баннер
        }
    }, []);

    const handleCookieAccept = () => {
        localStorage.setItem('cookie_consent', 'true');
        setCookieConsent(true);
    };

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

            {/* Баннер cookie — показывается, только если согласие ещё не дано */}
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
        </PageFrame>
    );
}