import React, { useEffect, useRef } from 'react';
import Chart from 'chart.js/auto';
import { getSalesByStoreId } from '../http/saleApi';

const SalesChart = ({ storeId }) => {
  const chartRef = useRef(null);

  useEffect(() => {
    const fetchDataAndDrawChart = async () => {
      try {
        const salesData = await getSalesByStoreId(storeId);
        if (salesData.length > 0) {
          const ctx = chartRef.current.getContext('2d');
          if (chartRef.current && chartRef.current.chart) {
            chartRef.current.chart.destroy();
          }

          const dailySales = {};
          salesData.forEach(sale => {
            const date = new Date(sale.saleDate).toLocaleDateString();
            const totalQuantity = sale.saleItems.reduce((total, item) => total + item.quantity, 0);
            if (dailySales[date]) {
              dailySales[date] += totalQuantity;
            } else {
              dailySales[date] = totalQuantity;
            }
          });

          const dates = Object.keys(dailySales);
          const amounts = Object.values(dailySales);

          chartRef.current.chart = new Chart(ctx, {
            type: 'bar',
            data: {
              labels: dates,
              datasets: [{
                label: 'Total Sales',
                data: amounts,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1,
              }]
            },
            options: {
              scales: {
                y: {
                  beginAtZero: true
                }
              }
            }
          });
        }
      } catch (error) {
        console.error('Error fetching sales data:', error);
      }
    };

    fetchDataAndDrawChart();

    return () => {
      if (chartRef.current && chartRef.current.chart) {
        chartRef.current.chart.destroy();
      }
    };
  }, [storeId]);

  return (
    <canvas ref={chartRef} width="400" height="200"></canvas>
  );
};

export default SalesChart;
