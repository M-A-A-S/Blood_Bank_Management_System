import { useEffect, useState } from "react";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import Button from "../../components/Button";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";
import { Link } from "react-router-dom";
import { formatDate } from "../../utils/utils";

const PatientsDashboard = () => {
  const [patients, setPatients] = useState([]);
  const [blood, setBlood] = useState([]);
  const [recordIdToDelete, setRecordIdToDelete] = useState(null);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  useEffect(() => {
    getBlood();
    getPatients();
  }, []);

  const getBlood = async () => {
    let result = await APIUtiliyWithJWT.get("/api/blood");
    if (result) {
      setBlood(result);
    }
  };

  const getPatients = async () => {
    let result = await APIUtiliyWithJWT.get("/api/patients");
    if (result) {
      setPatients(result);
    }
  };

  const deletePatient = async () => {
    if (recordIdToDelete) {
      let result = APIUtiliyWithJWT.delete(`/api/patients/${recordIdToDelete}`);
      if (result) {
        setPatients(
          patients.filter((patient) => patient.id !== recordIdToDelete)
        );
        showAlert("Data Deleted Successfuly", "success");
      } else {
        showAlert("Something Went Wrong", "error");
      }
    }

    handleCloseDialog();
  };

  const getBloodGroupName = (id) => {
    let bloodGroupName = "";
    blood &&
      blood.forEach((blood) => {
        if (blood.id === id) {
          bloodGroupName = blood.bloodGroupName;
        }
      });
    return bloodGroupName;
  };

  const handleOpenDialog = (recordId) => {
    setRecordIdToDelete(recordId);
    setIsDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setRecordIdToDelete(0);
    setIsDialogOpen(false);
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };

  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
  };

  return (
    <section className="section section-center">
      <div className="title" style={{ marginBottom: "2rem" }}>
        <h2>Patients Dashboard</h2>
        <div className="title-underline"></div>
      </div>
      <Link to="/patients/add" className="btn">
        Add New Patient
      </Link>
      <div className="table-container">
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Date Of Birth</th>
              <th>Gender</th>
              <th>Phone</th>
              <th>Blood Group</th>
              <th>Address</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>
            {patients &&
              patients.map((donor) => (
                <tr key={donor.id}>
                  <td>{donor.id}</td>
                  <td>{donor.name}</td>
                  <td>{formatDate(donor.dateOfBirth)}</td>
                  <td>{donor.isMale ? "Male" : "Female"}</td>
                  <td>{donor.phone}</td>
                  <td>{getBloodGroupName(donor.bloodGroupId)}</td>
                  <td>{donor.address}</td>
                  <td>
                    <Link to={`/patients/update/${donor.id}`} className="btn">
                      Edit
                    </Link>
                  </td>
                  <td>
                    <Button
                      onClick={() => handleOpenDialog(donor.id)}
                      children={"Delete"}
                    />
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
      <Dialog
        isOpen={isDialogOpen}
        title="Confirm Your Action"
        message="Are you sure you want to delete this record?"
        onConfirm={deletePatient}
        onCancel={handleCloseDialog}
      />
      <Alert
        isOpen={alertConfig.isOpen}
        message={alertConfig.message}
        type={alertConfig.type}
        duration={alertConfig.duration}
        onClose={closeAlert}
      />
    </section>
  );
};
export default PatientsDashboard;
