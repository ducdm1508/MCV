
import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import BookPage from './Pages/BookPage';
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/book" element={<BookPage/>} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
