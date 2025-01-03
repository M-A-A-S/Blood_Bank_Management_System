const Input = (props) => {
  const {
    label,
    errorMessage,
    value,
    onChange,
    type = "text",
    name,
    placeholder,
    ref = null,
    ...inputProps
  } = props;
  return (
    <div className="form-row">
      <label className="form-label">{label}</label>
      <input
        ref={ref}
        type={type}
        name={name}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        {...inputProps}
        className="form-input"
      />
      {errorMessage && <small className="form-alert">{errorMessage}</small>}
    </div>
  );
};
export default Input;
