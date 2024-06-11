import React, { useEffect, useState } from 'react';
import Menu from '../component/menu/menu';
import { getSalesByStoreId } from '../http/saleApi';
import { useTranslation } from 'react-i18next';

function Sale() {
    const { t } = useTranslation('sales');
    const [sales, setSales] = useState([]);
    const [storeId, setStoreId] = useState(null); 

    useEffect(() => {
        const storedShopId = localStorage.getItem('shopId');
        console.log(storedShopId); 
    
        if (storedShopId) {
          setStoreId(storedShopId);
        }
    }, []);

    useEffect(() => {
        const fetchSales = async () => {
            try {
                const salesData = await getSalesByStoreId(storeId);
                setSales(salesData);
            } catch (error) {
                console.error(t('error_fetching_sales'), error);
            }
        };

        if (storeId) {
            fetchSales();
        }
    }, [storeId]);

    return (
        <div className="sale-page">
            <div className="sale-menu">
                <Menu />
            </div>
            <div className="content">
                <div className="label-sale">{t('sale')}</div>
                <hr/>
                <div className="sales-list">
                    <table className="table">
                        <thead>
                            <tr>
                                <th>{t('sale_id')}</th>
                                <th>{t('sale_date')}</th>
                                <th>{t('employee')}</th>
                                <th>{t('product_name')}</th>
                                <th>{t('quantity')}</th>
                                <th>{t('price')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {sales.map(sale => (
                                <React.Fragment key={sale.saleId}>
                                    {sale.saleItems.map(item => (
                                        <tr key={item.saleItemId}>
                                            <td>{sale.saleId}</td>
                                            <td>{sale.saleDate ? new Date(sale.saleDate).toLocaleString() : ''}</td>
                                            <td>{sale.employee && `${sale.employee.firstName} ${sale.employee.lastName}`}</td>
                                            <td>{item.product && item.product.productName}</td>
                                            <td>{item.quantity}</td>
                                            <td>{item.quantity * item.price} $</td>
                                        </tr>
                                    ))}
                                </React.Fragment>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}

export default Sale;
