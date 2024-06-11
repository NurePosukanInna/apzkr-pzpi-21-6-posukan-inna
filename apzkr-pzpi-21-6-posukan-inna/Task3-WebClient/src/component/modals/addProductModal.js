import React, { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { fetchCategories } from '../../http/categoryApi';
import { fetchSuppliers } from '../../http/supplierApi';
import { useTranslation } from 'react-i18next';

function AddProductModal({ show, handleClose, formData = {}, handleChange, handleSubmit, availableCurrencies }) {
  const { t } = useTranslation('product');
  const [categories, setCategories] = useState([]);
  const [suppliers, setSuppliers] = useState([]);

  useEffect(() => {
    const fetchCategoriesData = async () => {
      try {
        const categoriesData = await fetchCategories();
        setCategories(categoriesData || []);
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };

    const fetchSuppliersData = async () => {
      try {
        const suppliersData = await fetchSuppliers();
        setSuppliers(suppliersData || []);
      } catch (error) {
        console.error('Error fetching suppliers:', error);
      }
    };

    fetchCategoriesData();
    fetchSuppliersData();
  }, []);

  const defaultFormData = {
    ProductName: '',
    Price: null,
    MinQuantity: 0,
    Volume: 0,
    MeasureOfUnits: '',
    IsFresh: false,
    ExpiryDate: '',
    Currency: availableCurrencies[0] || '',
    CategoryId: '',
    SupplierId: '',
  };

  const mergedFormData = { ...defaultFormData, ...formData };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>{t('add_product')}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form>
          <div className="form-group">
            <label>{t('name')}:</label>
            <input type="text" className="form-control" name="ProductName" value={mergedFormData.ProductName || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('price')}:</label>
            <input type="number" className="form-control" name="Price" value={mergedFormData.Price || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('min_quantity')}:</label>
            <input type="number" className="form-control" name="MinQuantity" value={mergedFormData.MinQuantity || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('currency')}:</label>
            <select className="form-control" name="Currency" value={mergedFormData.Currency || ''} onChange={handleChange}>
              {availableCurrencies.map(currency => (
                <option key={currency} value={currency}>{currency}</option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <label>{t('volume')}:</label>
            <input type="number" className="form-control" name="Volume" value={mergedFormData.Volume || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('measurement_unit')}:</label>
            <input type="text" className="form-control" name="MeasureOfUnits" value={mergedFormData.MeasureOfUnits || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('is_fresh')}:</label>
            <input type="checkbox" className="form-check-input" name="IsFresh" checked={mergedFormData.IsFresh} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('expiry_date')}:</label>
            <input type="date" className="form-control" name="ExpiryDate" value={mergedFormData.ExpiryDate || ''} onChange={handleChange} />
          </div> 
          <div className="form-group">
            <label>{t('category')}:</label>
            <select className="form-control" name="CategoryId" value={mergedFormData.CategoryId || ''} onChange={handleChange}>
              <option value="">{t('select_category')}</option>
              {categories.map(category => (
                <option key={category.categoryId} value={category.categoryId}>{category.categoryName}</option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <label>{t('supplier')}:</label>
            <select className="form-control" name="SupplierId" value={mergedFormData.SupplierId || ''} onChange={handleChange}>
              <option value="">{t('select_supplier')}</option>
              {suppliers.map(supplier => (
                <option key={supplier.supplierId} value={supplier.supplierId}>{supplier.address}</option>
              ))}
            </select>
          </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="primary" onClick={handleSubmit}>
          {t('add_product')}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default AddProductModal;
