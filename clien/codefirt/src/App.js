
import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import BookPage from './Pages/BookPage';
import Department from './Components/Department/Department';
import Lucturer from './Components/Department/Lucturer';
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/book" element={<BookPage/>} />
        <Route path='/' element = {<Department/>}/>
        <Route path='/lucturer' element = {<Lucturer/>}/>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
