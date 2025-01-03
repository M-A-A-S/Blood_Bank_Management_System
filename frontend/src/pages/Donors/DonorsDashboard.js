import { useEffect, useState } from "react";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import Button from "../../components/Button";
import Dialog from "../../components/Dialog";
import Alert from "../../components/Alert";
import { Link } from "react-router-dom";
import { formatDate } from "../../utils/utils";

const DonorsDashboard = () => {
  const [donors, setDonors] = useState([]);
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
    getDonors();
  }, []);

  const getBlood = async () => {
    let result = await APIUtiliyWithJWT.get("/api/blood");
    if (result) {
      setBlood(result);
    }
  };

  const getDonors = async () => {
    let result = await APIUtiliyWithJWT.get("/api/donors");
    if (result) {
      setDonors(result);
    }
  };

  const deleteDonor = async () => {
    if (recordIdToDelete) {
      let result = APIUtiliyWithJWT.delete(`/api/donors/${recordIdToDelete}`);
      if (result) {
        setDonors(donors.filter((donor) => donor.id !== recordIdToDelete));
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
        <h2>Donors Dashboard</h2>
        <div className="title-underline"></div>
      </div>
      <Link to="/donors/add" className="btn">
        Add New Donor
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
            {donors &&
              donors.map((donor) => (
                <tr key={donor.id}>
                  <td>{donor.id}</td>
                  <td>{donor.name}</td>
                  <td>{formatDate(donor.dateOfBirth)}</td>
                  <td>{donor.isMale ? "Male" : "Female"}</td>
                  <td>{donor.phone}</td>
                  <td>{getBloodGroupName(donor.bloodGroupId)}</td>
                  <td>{donor.address}</td>
                  <td>
                    <Link to={`/donors/update/${donor.id}`} className="btn">
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
        onConfirm={deleteDonor}
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
export default DonorsDashboard;
