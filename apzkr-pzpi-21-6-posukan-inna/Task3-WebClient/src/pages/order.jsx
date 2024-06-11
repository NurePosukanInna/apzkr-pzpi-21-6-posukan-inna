import React, { useState, useEffect } from 'react';
import { fetchAllSupplierRequestsForEmployee, fetchAllSupplierRequests, deleteSupplierRequest, updateSupplierRequest } from '../http/orderApi'; 
import { useAuth } from '../context/authContext';
import Menu from '../component/menu/menu';
import UpdateModal from '../component/modals/updateRequestModal';
import { useTranslation } from 'react-i18next';

function Order() {
  const { userId, employeeId, position } = useAuth();
  const { t } = useTranslation('order');
  const [supplierRequests, setSupplierRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState(null);

  useEffect(() => {
    async function fetchData() {
      setLoading(true);
      try {
        let data;
        if (position === 'Cashier') {
          data = await fetchAllSupplierRequestsForEmployee(employeeId);
        } else {
          if (userId !== null) {
            data = await fetchAllSupplierRequests(userId); 
          } else {
            data = [];
          }
        }
        setSupplierRequests(data);
        setError(null);
      } catch (error) {
        console.error('Error fetching data:', error);
        setError(t('fetch_data_error'));
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, [employeeId, position, userId, t]);

  const handleDelete = async (requestId) => {
    try {
      await deleteSupplierRequest(requestId);
      alert(t('request_deleted'));
      const updatedRequests = supplierRequests.filter(request => request.requestId !== requestId);
      setSupplierRequests(updatedRequests);
    } catch (error) {
      console.error('Error deleting supplier request:', error);
    }
  };

  const handleUpdate = (request) => {
    setSelectedRequest(request);
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setSelectedRequest(null);
  };

  const handleUpdateSubmit = async (updatedData) => {
    try {
      await updateSupplierRequest(selectedRequest.requestId, updatedData);
      alert(t('request_updated'));
      setShowModal(false);
      const updatedRequests = supplierRequests.map(request =>
        request.requestId === selectedRequest.requestId ? { ...request, ...updatedData } : request
      );
      setSupplierRequests(updatedRequests);
    } catch (error) {
      console.error('Error updating supplier request:', error);
    }
  };

  return (
    <div className='request-page'>
      <div className="request-menu">
        <Menu/>
      </div>
      <div className="content">
        <div className="label-employee">{t('label_supplier_request')}</div>
        <hr/>
        {loading ? (
          <p>{t('loading')}</p>
        ) : error ? (
          <p>{error}</p>
        ) : (
          <div>
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>{t('order_id')}</th>
                  <th>{t('store_name')}</th>
                  <th>{t('product_name')}</th>
                  <th>{t('quantity')}</th>
                  <th>{t('total_amount')}</th>
                  <th>{t('request_date')}</th>
                  <th>{t('request_status')}</th>
                  <th>{t('delivery_date')}</th>
                  <th>{t('actions')}</th>
                  <th>{t('update')}</th>
                </tr>
              </thead>
              <tbody>
                {supplierRequests.map(request => (
                  <tr key={request.requestId}>
                    <td>{request.requestId}</td>
                    <td>{request.storeProduct.store.storeName}</td>
                    <td>{request.storeProduct.product.productName}</td>
                    <td>{request.quantity}</td>
                    <td>{request.totalAmount}</td>
                    <td>{request.requestDate ? new Date(request.requestDate).toLocaleDateString() : ''}</td>
                    <td>{request.requestStatus}</td>
                    <td>{request.deliveryDate ? new Date(request.deliveryDate).toLocaleDateString() : t('not_delivered')}</td>
                    <td>
                      <button className='btn btn-danger' onClick={() => handleDelete(request.requestId)}>{t('delete')}</button>
                    </td>
                    <td>
                      <button className='btn btn-info' onClick={() => handleUpdate(request)}>{t('update')}</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <UpdateModal show={showModal} handleClose={handleCloseModal} handleUpdate={handleUpdateSubmit} request={selectedRequest} />
          </div>
        )}
      </div>
    </div>
  );
}

export default Order;
