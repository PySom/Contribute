import { BrowserRouter, NavLink, Route, Routes } from 'react-router-dom'

import Contributions from './pages/Contributions'
import Home from './pages/Home'

function App() {
return (
    <BrowserRouter>
      <div className="app-container">
        <header style={{ marginBottom: 20 }}>
          <nav className="site-nav" style={{ display: 'flex', gap: 12 }}>
            <NavLink to="/" end className={({ isActive }) => (isActive ? 'active' : '')}>
              Home
            </NavLink>
            <NavLink
              to="/contributions"
              className={({ isActive }) => (isActive ? 'active disabled' : 'disabled')}
            >
              Contributions
            </NavLink>
          </nav>
        </header>

        <main>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/contributions/:slug" element={<Contributions />} />
            <Route path="/contributions" element={<Contributions />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  )
}

export default App