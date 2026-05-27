import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { registerUser } from '../api/auth';
import { PageFrame } from '../components/PageFrame';

export function RegisterPage() {
    const navigate = useNavigate();
    const [form, setForm] = useState({
        username: '',
        password: '',
        confirmPassword: '',
    });
    const [agreed, setAgreed] = useState(false); // состояние чекбокса
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const onChange = (event) => {
        const { name, value } = event.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    const onSubmit = async (event) => {
        event.preventDefault();
        setError('');
        setSuccess('');

        if (!form.username.trim() || !form.password.trim() || !form.confirmPassword.trim()) {
            setError('Все поля обязательны для регистрации.');
            return;
        }

        if (form.password.length < 6) {
            setError('Пароль должен содержать минимум 6 символов.');
            return;
        }

        if (form.password !== form.confirmPassword) {
            setError('Пароли не совпадают.');
            return;
        }

        if (!agreed) {
            setError('Необходимо дать согласие на обработку персональных данных.');
            return;
        }

        try {
            setIsSubmitting(true);
            const response = await registerUser({
                username: form.username,
                password: form.password,
            });
            setSuccess(
                `Профиль ${response?.username || form.username} успешно создан. Теперь можно войти в аккаунт.`,
            );
            setTimeout(() => navigate('/login'), 600);
        } catch (submitError) {
            setError(submitError.message || 'Не удалось зарегистрироваться.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <PageFrame chip="NEW PLAYER">
            <section className="auth-wrapper">
                <h1 className="auth-title">Регистрация в Jilo</h1>
                <p className="auth-subtitle">
                    Создай аккаунт, чтобы находить тиммейтов по ролям, скиллу и игровым целям.
                </p>

                <form className="auth-form" onSubmit={onSubmit}>
                    <label className="label" htmlFor="username">
                        Имя пользователя
                        <input
                            className="input"
                            id="username"
                            name="username"
                            value={form.username}
                            onChange={onChange}
                            autoComplete="username"
                            placeholder="например, VoidCaptain"
                        />
                    </label>

                    <label className="label" htmlFor="password">
                        Пароль
                        <input
                            className="input"
                            id="password"
                            name="password"
                            type="password"
                            value={form.password}
                            onChange={onChange}
                            autoComplete="new-password"
                            placeholder="Минимум 6 символов"
                        />
                    </label>

                    <label className="label" htmlFor="confirmPassword">
                        Повторите пароль
                        <input
                            className="input"
                            id="confirmPassword"
                            name="confirmPassword"
                            type="password"
                            value={form.confirmPassword}
                            onChange={onChange}
                            autoComplete="new-password"
                            placeholder="Введите пароль еще раз"
                        />
                    </label>

                    {/* Чекбокс согласия на обработку ПД */}
                    <div style={{ margin: '16px 0', display: 'flex', alignItems: 'flex-start', gap: '8px' }}>
                        <input
                            type="checkbox"
                            id="privacy-consent"
                            checked={agreed}
                            onChange={(e) => setAgreed(e.target.checked)}
                            style={{ marginTop: '2px' }}
                        />
                        <label htmlFor="privacy-consent" style={{ fontSize: '14px', lineHeight: '1.4' }}>
                            Я даю согласие на обработку моих персональных данных (никнейм, идентификатор
                            аккаунта) на условиях{' '}
                            <Link to="/agreement" target="_blank" style={{ color: 'var(--accent)' }}>
                                Согласия на обработку
                            </Link>{' '}
                            и{' '}
                            <Link to="/policy" target="_blank" style={{ color: 'var(--accent)' }}>
                                Политики обработки персональных данных
                            </Link>
                        </label>
                    </div>

                    {error ? <div className="error-box">{error}</div> : null}
                    {success ? <div className="success-box">{success}</div> : null}

                    <button
                        className="button primary"
                        type="submit"
                        disabled={isSubmitting || !agreed} // кнопка неактивна без согласия
                    >
                        {isSubmitting ? 'Создаём аккаунт...' : 'Зарегистрироваться'}
                    </button>
                </form>

                <p className="auth-footer">
                    Уже есть аккаунт? <Link to="/login">Войти</Link>
                </p>
            </section>
        </PageFrame>
    );
}