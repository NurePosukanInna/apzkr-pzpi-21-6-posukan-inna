import React, { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { fetchCategories } from '../../http/categoryApi';
import { fetchSuppliers } from '../../http/supplierApi';
import { useTranslation } from 'react-i18next';

function UpdateProductModal({ show, handleClose, formData = {}, handleChange, handleSubmit, availableCurrencies }) {
  const { t } = useTranslation('product');
  const [prevFormData, setPrevFormData] = useState({});
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

  useEffect(() => {
    setPrevFormData(formData);
  }, [formData]);

  const mergedFormData = { ...prevFormData, ...formData, categoryId: formData.categoryId || '' };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>{t('update_product')}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form>
          <div className="form-group">
            <label>{t('name')}:</label>
            <input type="text" className="form-control" name="productName" value={mergedFormData.productName || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('price')}:</label>
            <input type="number" className="form-control" name="price" value={mergedFormData.price || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('min_quantity')}:</label>
            <input type="number" className="form-control" name="minQuantity" value={mergedFormData.minQuantity || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('quantity')}:</label>
            <input type="number" className="form-control" name="quantity" value={mergedFormData.quantity || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('volume')}:</label>
            <input type="number" className="form-control" name="volume" value={mergedFormData.volume || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('measurement_unit')}:</label>
            <input type="text" className="form-control" name="measureOfUnits" value={mergedFormData.measureOfUnits || ''} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('is_fresh')}:</label>
            <input type="checkbox" className="form-check-input" name="isFresh" checked={mergedFormData.isFresh} onChange={handleChange} />
          </div>
          <div className="form-group">
            <label>{t('expiry_date')}:</label>
            <input 
              type="date" 
              className="form-control" 
              name="expiryDate" 
              value={mergedFormData.expiryDate || ''} 
              onChange={handleChange} 
            />
          </div> 
          <div className="form-group">
            <label>{t('category')}:</label>
            <select className="form-control" name="categoryId" value={mergedFormData.categoryId || ''} onChange={handleChange}>
              <option value="">{t('select_category')}</option>
              {categories.map(category => (
                <option key={category.categoryId} value={category.categoryId}>{category.categoryName}</option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <label>{t('supplier')}:</label>
            <select className="form-control" name="supplierId" value={mergedFormData.supplierId || ''} onChange={handleChange}>
              <option value="">{t('select_supplier')}</option>
              {suppliers.map(supplier => (
                <option key={supplier.supplierId} value={supplier.supplierId}>{supplier.address}</option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <label>{t('currency')}:</label>
            <select className="form-control" name="currency" value={mergedFormData.currency || ''} onChange={handleChange}>
              {availableCurrencies.map(currency => (
                <option key={currency} value={currency}>{currency}</option>
              ))}
            </select>
          </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="primary" onClick={handleSubmit}>
          {t('update')}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default UpdateProductModal;
