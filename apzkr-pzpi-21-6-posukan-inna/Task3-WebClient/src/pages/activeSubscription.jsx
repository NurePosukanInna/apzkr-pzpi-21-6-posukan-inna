import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/authContext';
import { getActiveSubscriptionsForUser } from '../http/subscriptionApi';
import Menu from '../component/menu/menu';
import '../styles/subscription.css';
import { useTranslation } from 'react-i18next';

function ActiveSubscription() {
    const { userId } = useAuth();
    const [activeSubscription, setActiveSubscription] = useState(null);
    const { t } = useTranslation('activeSubscription');

    useEffect(() => {
        const fetchActiveSubscriptions = async () => {
            try {
                const activeSubs = await getActiveSubscriptionsForUser(userId);
                setActiveSubscription(activeSubs[0]);
            } catch (error) {
                console.error('Error fetching active subscriptions:', error);
            }
        };

        fetchActiveSubscriptions();
    }, [userId]);

    return (
        <div className="subscription-page">
            <div className="subscription-menu">
                <Menu />
            </div>
            <div className="content">
                <div className="label-subscription">{t('your_subscription')}</div>
                <hr />
                <div className="subscription-details">
                    {activeSubscription && (
                        <div className="subscription-item">
                            <p>{t('status')}: {activeSubscription.subscriptionStatus}</p>
                            <p>{t('start_date')}: {activeSubscription ? new Date(activeSubscription.startDate).toLocaleDateString() : ''}</p>
                            <p>{t('end_date')}: {activeSubscription ? new Date(activeSubscription.endDate).toLocaleDateString() : ''}</p>
                            <p>{t('name')}: {activeSubscription.subscriptionType.name}</p>
                            <p>{t('price')}: {activeSubscription.subscriptionType.price}</p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}

export default ActiveSubscription;
