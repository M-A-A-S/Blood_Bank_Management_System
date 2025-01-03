import { Link, useNavigate } from "react-router-dom";
import Button from "../../components/Button";
import Input from "../../components/Input";
import Navbar from "../../components/Navbar";
import Select from "../../components/Select";
import Alert from "../../components/Alert";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import { useEffect, useRef, useState } from "react";
import { formatDate } from "../../utils/utils";

const BloodTransfer = () => {
  const navigate = useNavigate();

  const [blood, setBlood] = useState([]);
  const [patients, setPatients] = useState([]);
  const [selectPatientId, setSelectPatientId] = useState(0);
  const [selectedBloodGroupId, setSelectedBloodGroupId] = useState(0);
  const [bloodGroup, setBloodGroup] = useState({});
  const [errors, setErrors] = useState({});
  const bloodGroupRef = useRef(null);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  useEffect(() => {
    getBlood();
    getPatients();
  }, []);

  useEffect(() => {
    handlePatientIdUpdate();
  }, [selectPatientId]);

  const handlePatientIdUpdate = async () => {
    if (selectPatientId) {
      let result = await APIUtiliyWithJWT.get(
        `/api/patients/${selectPatientId}`
      );
      if (result) {
        setSelectedBloodGroupId(result.bloodGroupId);
        setBloodGroup(
          await APIUtiliyWithJWT.get(`/api/blood/${result.bloodGroupId}`)
        );
        bloodGroupRef.current.value = getBloodGroupName(result.bloodGroupId);
      }
    }
  };

  const getPatients = async () => {
    let result = await APIUtiliyWithJWT.get("/api/patients");
    if (result) {
      setPatients(result);
    }
  };

  const getBlood = async () => {
    let result = await APIUtiliyWithJWT.get("/api/blood");
    if (result) {
      setBlood(result);
    }
  };

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };
  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/");
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

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = {};
    if (!selectPatientId) {
      validationErrors.patient = "Please select a patient";
    }
    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      transfer();
    }
  };

  const transfer = async () => {
    const patient = await APIUtiliyWithJWT.get(
      `/api/patients/${selectPatientId}`
    );
    let bloodGroup = await APIUtiliyWithJWT.get(
      `/api/blood/${selectedBloodGroupId}`
    );

    if (patient && bloodGroup) {
      let transferResult = await APIUtiliyWithJWT.put(
        `/api/blood/${selectedBloodGroupId}`,
        { ...bloodGroup, quantityInStock: bloodGroup.quantityInStock - 1 }
      );

      if (transferResult) {
        const result = await APIUtiliyWithJWT.post("/api/bloodTransfers", {
          patientId: patient.id,
          bloodGroupId: patient.bloodGroupId,
          transferDate: formatDate(new Date()),
        });

        if (result) {
          showAlert("Transfer Done Successfuly", "success");
        } else {
          bloodGroup = await APIUtiliyWithJWT.get(
            `/api/blood/${selectedBloodGroupId}`
          );
          await APIUtiliyWithJWT.put(`/api/blood/${selectedBloodGroupId}`, {
            ...bloodGroup,
            quantityInStock: bloodGroup.quantityInStock + 1,
          });
          showAlert("Something Went Wrong", "error");
        }
      }
    }
  };

  const handleDonorChange = (e) => {
    setSelectPatientId(+e.target.value);
  };

  return (
    <>
      <Navbar />
      <section className="section section-center">
        <form className="form" onSubmit={handleSubmit}>
          <h4
            style={{
              marginBottom: "1rem",
              textAlign: "center",
              color: "var(--primary-500)",
              fontWeight: "bold",
            }}
          >
            Transfer
          </h4>
          <Select
            label="Patient"
            value={selectPatientId}
            onChange={handleDonorChange}
            errorMessage={errors.patient}
            options={
              patients &&
              patients.map((patient) => {
                return { value: patient.id, label: patient.name };
              })
            }
          />
          <Input
            label="Blood Group"
            value={bloodGroupRef.current?.value}
            readOnly
            ref={bloodGroupRef}
          />
          {bloodGroup && bloodGroup.quantityInStock > 0 ? (
            <Button
              style={{ marginBottom: "0.5rem" }}
              onClick={handleSubmit}
              children={"Transfer"}
              className={"btn-block"}
            />
          ) : (
            <p
              className="form-alert"
              style={{ textAlign: "center", marginBottom: "0.5rem" }}
            >
              Not available
            </p>
          )}

          <Link
            style={{ textAlign: "center" }}
            className="btn btn-block"
            to="/donors"
          >
            Cancel
          </Link>
        </form>
        <Alert
          isOpen={alertConfig.isOpen}
          message={alertConfig.message}
          type={alertConfig.type}
          duration={alertConfig.duration}
          onClose={closeAlert}
        />
      </section>
    </>
  );
};
export default BloodTransfer;
