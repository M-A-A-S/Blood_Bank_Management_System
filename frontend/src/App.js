import { BrowserRouter, Route, Routes } from "react-router-dom";
import PrivateRoute from "./auth/PrivateRoute";
import Donors from "./pages/Donors";
import AddUpdateDonor from "./pages/Donors/AddUpdateDonor";
import Patients from "./pages/Patients";
import AddUpdatePatient from "./pages/Patients/AddUpdatePatient";
import Donate from "./pages/Donate";
import BloodTransfer from "./pages/BloodTransfer";
import Dashboard from "./pages/Dashboard";
import "./App.css";
import NotFound from "./pages/NotFound";
import Login from "./pages/Login";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route
          path="/"
          element={
            <PrivateRoute>
              <Donors></Donors>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/donors"
          element={
            <PrivateRoute>
              <Donors></Donors>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/donors/add"
          element={
            <PrivateRoute>
              <AddUpdateDonor></AddUpdateDonor>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/donors/update/:id"
          element={
            <PrivateRoute>
              <AddUpdateDonor></AddUpdateDonor>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/patients"
          element={
            <PrivateRoute>
              <Patients></Patients>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/patients/add"
          element={
            <PrivateRoute>
              <AddUpdatePatient></AddUpdatePatient>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/patients/update/:id"
          element={
            <PrivateRoute>
              <AddUpdatePatient></AddUpdatePatient>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/donate"
          element={
            <PrivateRoute>
              <Donate></Donate>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/bloodTransfer"
          element={
            <PrivateRoute>
              <BloodTransfer></BloodTransfer>
            </PrivateRoute>
          }
        ></Route>
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <Dashboard></Dashboard>
            </PrivateRoute>
          }
        ></Route>
        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
