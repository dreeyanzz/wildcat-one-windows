import './App.css'

const base = import.meta.env.BASE_URL

function App() {
  return (
    <div className="app">
      <header>
        <div className="container header-content">
          <div className="brand">
            <img src={`${base}cit-logo.png`} alt="CIT Logo" />
            <div className="brand-text">
              <span className="brand-title">Wildcat One</span>
              <span className="brand-subtitle">Student Portal</span>
            </div>
          </div>
          <nav>
            <a href="https://github.com/dreeyanzz/wildcat-one-windows" target="_blank" rel="noopener noreferrer" className="btn btn-secondary">
              GitHub Repository
            </a>
          </nav>
        </div>
      </header>

      <main>
        <section className="hero">
          <div className="container">
            <h1>Virtus in Scientia et Tecnologia</h1>
            <p className="hero-subtitle">
              The official centralized student portal for managing your academic journey. 
              Access grades, schedules, and more in one unified desktop experience.
            </p>
            <div className="hero-actions">
              <a href="https://github.com/dreeyanzz/wildcat-one-windows/releases" target="_blank" rel="noopener noreferrer" className="btn btn-primary">
                Download for Windows
              </a>
              <a href="https://wits-student.vercel.app/" target="_blank" rel="noopener noreferrer" className="btn btn-secondary">
                Use on the Web
              </a>
            </div>
          </div>
        </section>

        <div className="container">
          <section id="features" className="features-grid">
            <div className="card feature-card">
              <h3>For Users</h3>
              <p>Everything you need to get started with Wildcat One. Installation guides, troubleshooting, and user manuals.</p>
              <a href="#user-docs" className="link-arrow">View Documentation &rarr;</a>
            </div>
            
            <div className="card feature-card">
              <h3>For Developers</h3>
              <p>Contribute to the project. Access API documentation, architecture overviews, and setup guides.</p>
              <a href="#dev-docs" className="link-arrow">Developer Portal &rarr;</a>
            </div>
            
            <div className="card feature-card">
              <h3>Use on the Web</h3>
              <p>Don't want to install anything? Access Wildcat One directly from your browser — no download required.</p>
              <a href="https://wits-student.vercel.app/" target="_blank" rel="noopener noreferrer" className="link-arrow">Open Web App &rarr;</a>
            </div>
            
            <div className="card feature-card">
              <h3>Latest Release</h3>
              <p>Version 1.0.0 is now available. Improved performance, new dashboard analytics, and bug fixes.</p>
              <a href="#releases" className="link-arrow">Release Notes &rarr;</a>
            </div>
          </section>

          <section className="card" style={{ textAlign: 'center', marginBottom: '4rem' }}>
            <h2 style={{ marginBottom: '1rem' }}>Open Source</h2>
            <p style={{ maxWidth: '600px', margin: '0 auto 2rem' }}>
              Wildcat One is proudly open source. Join our community of student developers building the future of academic tools.
            </p>
            <a href="https://github.com/dreeyanzz/wildcat-one-windows" target="_blank" rel="noopener noreferrer" className="btn btn-primary">
              Contribute on GitHub
            </a>
          </section>
        </div>
      </main>

      <footer>
        <div className="container">
          <p>&copy; {new Date().getFullYear()} Wildcat One Windows. Licensed under MIT.</p>
          <p style={{ fontSize: '0.85rem', marginTop: '0.5rem' }}>
            Developed by <a href="https://facebook.com/dreeyanzz" target="_blank" rel="noopener noreferrer" style={{ color: 'var(--primary-maroon)', fontWeight: 600 }}>Adrian Seth Tabotabo</a>
          </p>
        </div>
      </footer>
    </div>
  )
}

export default App
