import { Link } from "react-router-dom";
const NotFound = () => {
  return (
    <section className="section section-center">
      <div className="not-found-container">
        <div>
          <h1 className="not-found-title">404 - Page Not Found</h1>
          <p className="not-found-message">
            Oops! The page you're looking for doesn't seem to exist.
          </p>
          <Link to="/" className="btn">
            Go back to Home
          </Link>
        </div>
      </div>
    </section>
  );
};
export default NotFound;
