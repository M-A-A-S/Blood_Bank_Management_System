const Select = ({
  options,
  name,
  value,
  onChange,
  className,
  errorMessage,
  label,
}) => {
  return (
    <div className="form-row">
      <label className="form-label">{label}</label>
      <select
        className={`form-input ${className}`}
        name={name}
        value={value}
        onChange={onChange}
      >
        {options &&
          options.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
      </select>
      {errorMessage && <small className="form-alert">{errorMessage}</small>}
    </div>
  );
};
export default Select;
