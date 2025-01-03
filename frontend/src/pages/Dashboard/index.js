import { useEffect, useState } from "react";
import Navbar from "../../components/Navbar";
import { APIUtiliyWithJWT } from "../../utils/apiClient";

const Dashboard = () => {
  const [numberOfPatients, setNumberOfPatients] = useState(0);
  const [numberOfDonors, setNumberOfDonors] = useState(0);
  const [numberOfBloodTransfers, setNumberOfBloodTransfers] = useState(0);
  const [blood, setBlood] = useState([]);

  useEffect(() => {
    getNumberOfPatients();
    getNumberOfDonors();
    getNumberOfBloodTransfers();
    getBlood();
  }, []);

  const getBlood = async () => {
    let result = await APIUtiliyWithJWT.get("/api/blood");
    if (result) {
      setBlood(result);
    }
  };

  const getNumberOfPatients = async () => {
    let result = await APIUtiliyWithJWT.get("/api/patients/NumberOfPatients");
    if (result) {
      setNumberOfPatients(result);
    } else {
      setNumberOfPatients(0);
    }
  };

  const getNumberOfDonors = async () => {
    let result = await APIUtiliyWithJWT.get("/api/donors/NumberOfDonors");
    if (result) {
      setNumberOfDonors(result);
    } else {
      setNumberOfDonors(0);
    }
  };

  const getNumberOfBloodTransfers = async () => {
    let result = await APIUtiliyWithJWT.get(
      "/api/bloodTransfers/NumberOfBloodTransfers"
    );
    if (result) {
      setNumberOfBloodTransfers(result);
    } else {
      setNumberOfBloodTransfers(0);
    }
  };

  return (
    <>
      <Navbar />
      <section className="section section-center">
        <div className="title" style={{ marginBottom: "2rem" }}>
          <h2>Dashboard</h2>
          <div className="title-underline"></div>
        </div>

        <div className="table-container">
          <table
            style={{
              minWidth: "var(--fixed-width)",
              textAlign: "center",
              margin: "0 auto",
            }}
          >
            <thead>
              <tr>
                <th>Id</th>
                <th>Blood Group Name</th>
                <th>quantity In Stock</th>
              </tr>
            </thead>
            <tbody>
              {blood &&
                blood.map((blood) => {
                  return (
                    <tr key={blood.id}>
                      <td>{blood.id}</td>
                      <td>{blood.bloodGroupName}</td>
                      <td>{blood.quantityInStock}</td>
                    </tr>
                  );
                })}
            </tbody>
          </table>
        </div>

        <div className="dashboard-container">
          <div className="dashboard-card">
            <h4>Donors</h4>
            <p>{numberOfDonors}</p>
          </div>
          <div className="dashboard-card">
            <h4>Patients</h4>
            <p>{numberOfPatients}</p>
          </div>
          <div className="dashboard-card">
            <h4>Blood Transfers</h4>
            <p>{numberOfBloodTransfers}</p>
          </div>
        </div>
      </section>
    </>
  );
};
export default Dashboard;
