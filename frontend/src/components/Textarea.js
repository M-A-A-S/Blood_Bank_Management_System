const Textarea = (props) => {
  const {
    label,
    errorMessage,
    value,
    onChange,
    name,
    placeholder,
    ...inputProps
  } = props;
  return (
    <div className="form-row">
      <label className="form-label">{label}</label>
      <textarea
        type="textarea"
        name={name}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        {...inputProps}
        className="form-textarea"
      ></textarea>
      {errorMessage && <small className="form-alert">{errorMessage}</small>}
    </div>
  );
};
export default Textarea;
