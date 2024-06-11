import React from 'react';
import { useTranslation } from 'react-i18next';
import Menu from '../component/menu/menu';
import analysisImage from '../assets/analysis1.avif';
import analysisImage2 from '../assets/analysis2.avif';
import '../styles/dashboard.css';
import { Link } from 'react-router-dom';

function Dashboard() {
  const { t } = useTranslation('dashboard');

  return (
    <div className='dashboard-page'>
      <div className="dashboard">
        <Menu />
      </div>
      <div className="content">
        <div className="label-dashboard">{t('dashboard')}</div>
        <hr/>
        <div className='dashboard-info'>
          <div className='dashboard-recommend'>
            <img className='analysis-image' src={analysisImage} alt="Analysis" />
            <div className="text-next-to-image">
              {t('createOrders')}
              <div className="additional-text">
                {t('createOrdersDescription')}
              </div>
              <Link to="/order">
                <button className="btn btn-success">
                  {t('createOrderButton')}
                </button> 
                </Link>
           </div>
          </div>

          <div className='dashboard-recommend'>
            <img className='analysis-image' src={analysisImage2} alt="Analysis" />
            <div className="text-next-to-image">
              {t('analyzeSales')}
              <div className="additional-text">
                {t('analyzeSalesDescription')}
              </div>
              <Link to="/sale">
                <button className="btn btn-success">
                  {t('displaySalesButton')}
                </button> 
                </Link>            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
