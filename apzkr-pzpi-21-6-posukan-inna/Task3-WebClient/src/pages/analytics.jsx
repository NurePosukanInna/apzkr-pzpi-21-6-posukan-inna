import React, { useState, useEffect } from 'react';
import Menu from '../component/menu/menu';
import SalesChart from '../component/salesChart';
import { fetchAllStores } from '../http/shopApi';
import { useAuth } from '../context/authContext';
import { Form } from 'react-bootstrap';
import { useTranslation } from 'react-i18next';

function Analytics() {
  const [storeId, setStoreId] = useState(null);
  const [stores, setStores] = useState([]);
  const { userId } = useAuth();
  const { t } = useTranslation('analytics');

  useEffect(() => {
    fetchAllStores(userId)
      .then(data => {
        if (data) {
          setStores(data);
          if (!storeId && data.length > 0) {
            setStoreId(data[0].storeId);
          }
        }
      })
      .catch(error => {
        console.error('Error fetching stores data:', error);
      });
  }, [storeId]);

  const handleStoreChange = (event) => {
    setStoreId(event.target.value);
  };

  return (
    <div className='analytics-page'>
      <div className="analytics">
        <Menu />
      </div>
      <div className="content">
        <div className="label-analytics">{t('store_analytics')}</div>
        <hr/>
        <Form.Select value={storeId} onChange={handleStoreChange}>
          {stores.map(store => (
            <option key={store.storeId} value={store.storeId}>{store.storeName}</option>
          ))}
        </Form.Select>
        {storeId && <SalesChart storeId={storeId} />}
      </div>
    </div>
  );
}

export default Analytics;
