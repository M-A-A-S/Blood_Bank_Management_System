import { useEffect, useRef, useState } from "react";
import Navbar from "../../components/Navbar";
import Select from "../../components/Select";
import Input from "../../components/Input";
import Button from "../../components/Button";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import Alert from "../../components/Alert";
import { Link, useNavigate } from "react-router-dom";

const Donate = () => {
  const navigate = useNavigate();

  const [blood, setBlood] = useState([]);
  const [donors, setDonors] = useState([]);
  const [selectDonorId, setSelectDonorId] = useState(0);
  const [selectedBloodGroupId, setSelectedBloodGroupId] = useState(0);
  const [errors, setErrors] = useState({});
  const bloodGroupRef = useRef(null);
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });

  useEffect(() => {
    getBlood();
    getDonors();
  }, []);

  useEffect(() => {
    handleDonorIdUpdate();
  }, [selectDonorId]);

  const handleDonorIdUpdate = async () => {
    if (selectDonorId) {
      let result = await APIUtiliyWithJWT.get(`/api/donors/${selectDonorId}`);
      if (result) {
        setSelectedBloodGroupId(result.bloodGroupId);
        bloodGroupRef.current.value = getBloodGroupName(result.bloodGroupId);
      }
    }
  };

  const getDonors = async () => {
    let result = await APIUtiliyWithJWT.get("/api/donors");
    if (result) {
      setDonors(result);
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
    if (!selectDonorId) {
      validationErrors.donor = "Please select a donor";
    }
    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      donate();
    }
  };

  const donate = async () => {
    let bloodGroup = await APIUtiliyWithJWT.get(
      `/api/blood/${selectedBloodGroupId}`
    );
    if (bloodGroup) {
      let result = await APIUtiliyWithJWT.put(
        `/api/blood/${selectedBloodGroupId}`,
        { ...bloodGroup, quantityInStock: bloodGroup.quantityInStock + 1 }
      );
      if (result) {
        showAlert("Donate Done Successfuly", "success");
      } else {
        showAlert("Something Went Wrong", "error");
      }
    }
  };

  const handleDonorChange = (e) => {
    setSelectDonorId(+e.target.value);
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
            Donate
          </h4>
          <Select
            label="Donor"
            value={selectDonorId}
            onChange={handleDonorChange}
            errorMessage={errors.donor}
            options={
              donors &&
              donors.map((donor) => {
                return { value: donor.id, label: donor.name };
              })
            }
          />
          <Input
            label="Blood Group"
            value={bloodGroupRef.current?.value}
            readOnly
            ref={bloodGroupRef}
          />
          <Button
            style={{ marginBottom: "0.5rem" }}
            onClick={handleSubmit}
            children={"Donate"}
            className={"btn-block"}
          />
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
export default Donate;
