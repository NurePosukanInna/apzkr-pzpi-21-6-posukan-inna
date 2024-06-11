import React, { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { useTranslation } from 'react-i18next';

function UpdateRequestModal({ show, handleClose, handleUpdate, request }) {
  const { t } = useTranslation("request");
  const [formData, setFormData] = useState({
    quantity: '',
    requestStatus: '',
  });

  useEffect(() => {
    if (request) {
      setFormData({
        quantity: request.quantity,
        requestStatus: request.requestStatus,
      });
    }
  }, [request]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    handleUpdate(formData);
    handleClose();
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>{t('update_request')}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="quantity">{t('quantity')}:</label>
            <input
              type="number"
              className="form-control"
              id="quantity"
              name="quantity"
              value={formData.quantity}
              onChange={handleChange}
            />
          </div>
          <div className="form-group">
            <label htmlFor="requestStatus">{t('request_status')}:</label>
            <select
              className="form-control"
              id="requestStatus"
              name="requestStatus"
              value={formData.requestStatus}
              onChange={handleChange}
            >
              <option value="Pending">{t('pending')}</option>
              <option value="Completed">{t('completed')}</option>
            </select>
          </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>
          {t('close')}
        </Button>
        <Button variant="primary" onClick={handleSubmit}>
          {t('save_changes')}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default UpdateRequestModal;
