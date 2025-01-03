const Button = ({ children, onClick, className, ...otherProps }) => {
  return (
    <button className={`btn ${className}`} onClick={onClick} {...otherProps}>
      {children}
    </button>
  );
};
export default Button;
