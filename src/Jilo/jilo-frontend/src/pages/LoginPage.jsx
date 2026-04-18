import { useState } from 'react'
import { Link } from 'react-router-dom'
import { loginUser } from '../api/auth'
import { PageFrame } from '../components/PageFrame'

export function LoginPage() {
  const [form, setForm] = useState({ username: '', password: '' })
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

    if (!form.username.trim() || !form.password.trim()) {
      setError('Введите имя пользователя и пароль.')
      return
    }

    try {
      setIsSubmitting(true)
      await loginUser(form)
      setSuccess('Вход выполнен успешно. Теперь можно переходить к поиску команды.')
    } catch (submitError) {
      setError(submitError.message || 'Не удалось выполнить вход.')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <PageFrame chip="AUTH">
      <section className="auth-wrapper">
        <h1 className="auth-title">Вход в Jilo</h1>
        <p className="auth-subtitle">
          Авторизуйся и начни собирать команду под свой стиль игры.
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
              placeholder="например, ShadowApex"
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
              autoComplete="current-password"
              placeholder="Введите пароль"
            />
          </label>

          {error ? <div className="error-box">{error}</div> : null}
          {success ? <div className="success-box">{success}</div> : null}

          <button className="button primary" type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Входим...' : 'Войти'}
          </button>
        </form>

        <p className="auth-footer">
          Нет аккаунта? <Link to="/register">Зарегистрироваться</Link>
        </p>
      </section>
    </PageFrame>
  )
}
