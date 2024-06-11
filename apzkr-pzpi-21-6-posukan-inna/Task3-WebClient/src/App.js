import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import "./../node_modules/bootstrap/dist/css/bootstrap.min.css";
import '@fortawesome/fontawesome-free/css/all.min.css';
import Dashboard from './pages/dashboard';
import Shop from './pages/shop';
import Product from './pages/product';
import Auth from './pages/auth';
import Employee from './pages/employee';
import Defective from './pages/defective';
import Order from './pages/order';
import Sale from './pages/sale';
import Analytics from './pages/analytics';
import Subscription from './pages/subscription';
import ActiveSubscription from './pages/activeSubscription';

function App() {
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path='/' element={<Auth/>} />
          <Route path='/dashboard' element={<Dashboard/>} />
          <Route path='/shop' element={<Shop/>} />
          <Route path="/shop/:shopId" element={<Product />} />
          <Route path='/employee' element={<Employee/>} />
          <Route path='/defective' element={<Defective/>} />
          <Route path='/order' element={<Order/>} />
          <Route path='/sale' element={<Sale/>} />
          <Route path='/chart' element={<Analytics/>} />
          <Route path='/subscription' element={<Subscription/>} />
          <Route path='/active' element={<ActiveSubscription/>} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
