import { useState } from 'react'
import { Link } from 'react-router-dom'
import { registerUser } from '../api/auth'
import { PageFrame } from '../components/PageFrame'

export function RegisterPage() {
  const [form, setForm] = useState({ email: '', username: '', password: '' })
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  const onChange = (event) => {
    const { name, value } = event.target
    setForm((prev) => ({ ...prev, [name]: value }))
  }

  const onSubmit = async (event) => {
    event.preventDefault()
    setError('')
    setSuccess('')

    if (!form.email.trim() || !form.username.trim() || !form.password.trim()) {
      setError('Все поля обязательны для регистрации.')
      return
    }

    if (form.password.length < 6) {
      setError('Пароль должен содержать минимум 6 символов.')
      return
    }

    try {
      setIsSubmitting(true)
      const response = await registerUser(form)
      setSuccess(
        `Профиль ${response?.username || form.username} успешно создан. Теперь можно войти в аккаунт.`,
      )
    } catch (submitError) {
      setError(submitError.message || 'Не удалось зарегистрироваться.')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <PageFrame chip="NEW PLAYER">
      <section className="auth-wrapper">
        <h1 className="auth-title">Регистрация в Jilo</h1>
        <p className="auth-subtitle">
          Создай аккаунт, чтобы находить тиммейтов по ролям, скиллу и игровым целям.
        </p>

        <form className="auth-form" onSubmit={onSubmit}>
          <label className="label" htmlFor="email">
            Email
            <input
              className="input"
              id="email"
              name="email"
              type="email"
              value={form.email}
              onChange={onChange}
              autoComplete="email"
              placeholder="you@example.com"
            />
          </label>

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

          {error ? <div className="error-box">{error}</div> : null}
          {success ? <div className="success-box">{success}</div> : null}

          <button className="button primary" type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Создаём аккаунт...' : 'Зарегистрироваться'}
          </button>
        </form>

        <p className="auth-footer">
          Уже есть аккаунт? <Link to="/login">Войти</Link>
        </p>
      </section>
    </PageFrame>
  )
}
