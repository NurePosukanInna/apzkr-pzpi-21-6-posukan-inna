import React, { useState, useEffect } from 'react';
import { fetchAllEmployees, addEmployee, deleteEmployee, updateEmployee } from '../http/employeeApi'; 
import Menu from '../component/menu/menu';
import AddEmployeeModal from '../component/modals/addEmployeeModal';
import UpdateEmployeeModal from '../component/modals/updateEmployeeModal';
import '../styles/employee.css';
import { useAuth } from '../context/authContext';
import { useTranslation } from 'react-i18next';

function Employee() {
  const [employee, setEmployee] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showUpdateModal, setShowUpdateModal] = useState(false);
  const [formData, setFormData] = useState({
    email: '',
    position: '',
    firstName: '',
    lastName: '',
    password: ''
  });
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [showSuccessAlert, setShowSuccessAlert] = useState(false); 

  const { userId } = useAuth();
  const { t } = useTranslation('employee');

  useEffect(() => {
    fetchData();
  }, [userId]);

  useEffect(() => {
    if (showSuccessAlert) {
      const timeout = setTimeout(() => {
        setShowSuccessAlert(false);
      }, 1200);

      return () => clearTimeout(timeout);
    }
  }, [showSuccessAlert]);

  const fetchData = async () => {
    try {
      const data = await fetchAllEmployees(userId);
      setEmployee(data); 
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };

  const handleAddUserClick = () => {
    setShowAddModal(true);
  };

  const handleCloseAddModal = () => {
    setShowAddModal(false);
  };

  const handleCloseUpdateModal = () => {
    setShowUpdateModal(false);
  };

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async () => {
    try {
      await addEmployee(formData);
      
      alert(t('employee_successfully_added'));
      setShowAddModal(false);
      
      await fetchData();
      
      setFormData({
        email: '',
        position: '',
        firstName: '',
        lastName: '',
        password: ''
      });
    } catch (error) {
      console.error('Error adding employee:', error);
    }
  };


  const handleDeleteUser = async (employeeId) => {
    try {
      await deleteEmployee(employeeId);
      alert(t('employee_successfully_deleted'));
      await fetchData();
    } catch (error) {
      console.error('Error deleting employee:', error);
    }
  };
  
  const handleUpdateUser = (employeeId) => {
    const selected = employee.find(emp => emp.employeeId === employeeId);
    setSelectedEmployee(selected);
    setShowUpdateModal(true);
  };
  
  const handleUpdateEmployee = async (employeeId, updatedData) => {
    try {
      await updateEmployee(employeeId, updatedData);
      setShowSuccessAlert(true); 
      setShowUpdateModal(false);
      await fetchData();
    } catch (error) {
      console.error('Error updating employee:', error);
    }
  };
    
  return (
    <div className='employee-page'>
      <div className="employee-menu">
        <Menu />
      </div>
      <div className="content">
        <div className="label-employee">{t('label_employee')}</div>
        <hr/>
        <div className="action" style={{ marginBottom: '20px' }}>
          <button className="btn btn-success" onClick={handleAddUserClick}>{t('add_user')}</button>
        </div>
        <table className="table table-striped">
          <thead>
            <tr>
              <th>{t('email')}</th>
              <th>{t('position')}</th>
              <th>{t('first_name')}</th>
              <th>{t('last_name')}</th>
              <th>{t('store')}</th>
              <th>{t('action')}</th>
              <th>{t('update')}</th>
            </tr>
          </thead>
          <tbody>
            {employee.map((employee) => (
              <tr key={employee.employeeId}>
                <td>{employee.email}</td>
                <td>{employee.position}</td>
                <td>{employee.firstName}</td>
                <td>{employee.lastName}</td>
                <td>{employee.store ? employee.store.storeName : t('not_available')}</td>
                <td>
                  <button className='btn btn-danger' onClick={() => handleDeleteUser(employee.employeeId)}>{t('delete')}</button>
                </td>
                <td>
                  <button className='btn btn-info' onClick={() => handleUpdateUser(employee.employeeId)}>{t('update')}</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <AddEmployeeModal 
          show={showAddModal} 
          handleClose={handleCloseAddModal} 
          formData={formData} 
          handleChange={handleChange} 
          handleSubmit={handleSubmit} 
        />
        <UpdateEmployeeModal 
          show={showUpdateModal} 
          handleClose={handleCloseUpdateModal} 
          employeeData={selectedEmployee} 
          handleUpdate={handleUpdateEmployee} 
        />
      </div>
      {showSuccessAlert && (
        <div className="bottom-alert">
          <div className="alert alert-success" role="alert">
            {t('employee_successfully_updated')}
          </div>
        </div>
      )}
    </div>
  );
}
export default Employee;