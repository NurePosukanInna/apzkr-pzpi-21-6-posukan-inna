import React from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { useTranslation } from 'react-i18next';

function ShopModal({ show, handleClose, formData, handleChange, handleSubmit }) {
  const { t } = useTranslation('shop');

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>{t('add_shop')}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form>
          <div className="form-group">
            <label>{t('name')}:</label>
            <input type="text" className="form-control" name="storeName" value={formData.storeName} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('address')}:</label>
            <input type="text" className="form-control" name="address" value={formData.address} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('phone_number')}:</label>
            <input type="text" className="form-control" name="phoneNumber" value={formData.phoneNumber} onChange={handleChange} />
          </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="btn btn-success" onClick={handleSubmit}>
          {t('add_shop')}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default ShopModal;
