import React, { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { fetchAllStores } from '../../http/shopApi';
import { useAuth } from '../../context/authContext';
import { useTranslation } from 'react-i18next'; 

function AddEmployeeModal({ show, handleClose, formData, handleChange, handleSubmit }) {
  const { userId } = useAuth();
  const { t } = useTranslation('employee');
  const [stores, setStores] = useState([]);

  useEffect(() => {
    if (show && userId) {
      fetchAllStores(userId).then(fetchedStores => {
        setStores(fetchedStores);
      });
    }
  }, [show, userId]);

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>{t('add_employee')}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form>
          <div className="form-group">
            <label>{t('email')}:</label>
            <input type="email" className="form-control" name="email" value={formData.email} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('password')}:</label>
            <input type="password" className="form-control" name="password" value={formData.password} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('position')}:</label>
            <select className="form-control" name="position" value={formData.position} onChange={handleChange}>
              <option value="">{t('select_role')}</option>
              <option value="Cashier">{t('cashier')}</option>
              <option value="Store Manager">{t('store_manager')}</option>
            </select>
          </div>
          <div className="form-group">
            <label>{t('first_name')}:</label>
            <input type="text" className="form-control" name="firstName" value={formData.firstName} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('last_name')}:</label>
            <input type="text" className="form-control" name="lastName" value={formData.lastName} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('store')}:</label>
            <select className="form-control" name="storeId" value={formData.storeId} onChange={handleChange}>
              <option value="">{t('select_store')}</option>
              {stores.map(store => (
                <option key={store.storeId} value={store.storeId}>{store.storeName}</option>
              ))}
            </select>
          </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="primary" onClick={handleSubmit}>
          {t('add_user')}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default AddEmployeeModal;
