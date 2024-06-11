import React, { useEffect, useState } from 'react';
import Menu from '../component/menu/menu';
import { getSubscriptionTypes, addSubscriptionToUser } from '../http/subscriptionApi';
import '../styles/subscription.css';
import { useAuth } from '../context/authContext';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

function Subscription() {
  const [subscriptionTypes, setSubscriptionTypes] = useState([]);
  const { userId } = useAuth();
  const { t } = useTranslation('subscription');

  const fetchSubscriptionTypes = async () => {
    try {
      const types = await getSubscriptionTypes();
      setSubscriptionTypes(types);
    } catch (error) {
      console.error(t('error_fetching_subscription_types'), error);
    }
  };

  useEffect(() => {
    fetchSubscriptionTypes();
  }, []);

  const handleBuyNow = async (subscriptionTypeId) => {
    try {
      const subscriptionData = { subscriptionTypeId };
      await addSubscriptionToUser(userId, subscriptionData);
      fetchSubscriptionTypes();
    } catch (error) {
      console.error(t('error_buying_subscription'), error);
    }
  };

  return (
    <div className="subscription-page">
      <div className="subscription-menu">
        <Menu />
      </div>
      <div className="content">
        <div className="label-subscription">{t('label_subscription')}</div>
        <hr />
        <div className="subscription-types">
          {subscriptionTypes.map((type, index) => (
            <div key={index} className="subscription-item">
              <div className="subscription-item-inner">
                <h3>{type.name}</h3>
                <p>{type.description}</p>
                <p className="subscription-price">{type.price}$ <small>{t('per_month')}</small></p>
                <ul className="features-list">
                  <div className="label-subscription-features">{t('subscription_features')}</div>
                  {type.name === 'Premium' ? (
                    <li>{t('all_functions')}</li>
                  ) : (
                    <li>{t('all_functions_except_analytics')}</li>
                  )}
                </ul>
                <Link to="/active">
                <button className="btn btn-success" onClick={() => handleBuyNow(type.subscriptionTypeId)}>{t('buy_now')}</button>
                </Link>
              </div>
            </div>
          ))}
        </div>        
      </div>
    </div>
  );
}

export default Subscription;
