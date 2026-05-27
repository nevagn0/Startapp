import { Link } from 'react-router-dom'
import { PageFrame } from '../components/PageFrame'

export function PolicyPage() {
    return (
        <PageFrame chip="PRIVACY">
            <div id="center">
                <div style={{ maxWidth: '720px', width: '100%', textAlign: 'left', padding: '0 24px' }}>
                    <h1>Политика обработки персональных данных</h1>

                    <div className="ticks" style={{ margin: '24px 0' }} />

                    <section>
                        <h2>1. Общие положения</h2>
                        <p>
                            Настоящая Политика определяет порядок обработки и защиты персональных данных
                            Пользователей сайта <strong>Jilo</strong> (далее — Сайт).
                        </p>
                        <p>
                            Оператор: <strong>Адышкин Сергей Сергеевич</strong>
                            e‑mail: <a href="mailto:no-email">no-email</a>.
                        </p>
                    </section>

                    <div className="ticks" style={{ margin: '16px 0' }} />

                    <section>
                        <h2>2. Какие данные мы обрабатываем</h2>
                        <p>Исключительно:</p>
                        <ul>
                            <li>Никнейм (псевдоним);</li>
                            <li>Идентификатор аккаунта (ID), сгенерированный на стороне Сайта.</li>
                        </ul>
                        <p>
                            Другие данные (IP‑адреса, файлы cookie, данные об устройстве и местоположении) не собираются.
                        </p>
                    </section>

                    <div className="ticks" style={{ margin: '16px 0' }} />

                    <section>
                        <h2>3. Цели обработки</h2>
                        <p>
                            Данные нужны для: создания учётной записи, аутентификации, предоставления сервиса поиска тиммейтов.
                        </p>
                    </section>

                    <div className="ticks" style={{ margin: '16px 0' }} />

                    <section>
                        <h2>4. Принципы обработки</h2>
                        <p>
                            Обработка ведётся на законной и справедливой основе, только для указанных целей, объём данных минимален.
                        </p>
                    </section>

                    <div className="ticks" style={{ margin: '16px 0' }} />

                    <section>
                        <h2>5. Права Пользователя</h2>
                        <p>
                            Пользователь вправе получить информацию о своих данных, требовать их уточнения, блокировки или уничтожения,
                            а также отозвать согласие, направив запрос на <a href="mailto:no-email">no-email</a>.
                        </p>
                    </section>

                    <div className="ticks" style={{ margin: '16px 0' }} />

                    <section>
                        <h2>6. Обязанности Оператора</h2>
                        <p>
                            Мы гарантируем конфиденциальность, не передаём данные третьим лицам, уведомляем Роскомнадзор о начале
                            обработки, принимаем все необходимые меры защиты.
                        </p>
                    </section>

                    <div className="ticks" style={{ margin: '16px 0' }} />

                    <section>
                        <h2>7. Заключительные положения</h2>
                        <p>
                            Политика действует бессрочно, актуальная редакция размещается на Сайте.
                            Продолжение использования Сайта означает согласие с Политикой.
                        </p>
                    </section>

                    <div className="ticks" style={{ margin: '24px 0' }} />

                    <div id="next-steps" style={{ marginTop: '16px' }}>
                        <div style={{ padding: '16px' }}>
                            <Link to="/" style={{ color: 'var(--accent)', textDecoration: 'none' }}>
                                ← Вернуться на главную
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </PageFrame>
    )
}