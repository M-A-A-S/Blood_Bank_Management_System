const Dialog = ({ isOpen, title, message, onConfirm, onCancel }) => {
  if (!isOpen) {
    return null;
  }

  return (
    <div className="confirm-dialog-overlay">
      <div className="confirm-dialog">
        <h4>{title}</h4>
        <p>{message}</p>
        <div className="dialog-actions">
          <button onClick={onConfirm} className="btn btn-danger">
            Confirm
          </button>
          <button onClick={onCancel} className="btn btn-success">
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};

export default Dialog;
