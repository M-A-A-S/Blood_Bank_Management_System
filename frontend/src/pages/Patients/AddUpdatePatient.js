import { useEffect, useState } from "react";
import Input from "../../components/Input";
import { formatDate } from "../../utils/utils";
import Select from "../../components/Select";
import Alert from "../../components/Alert";
import Textarea from "../../components/Textarea";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import Button from "../../components/Button";
import { Link, useNavigate, useParams } from "react-router-dom";

const AddUpdatePatient = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [patient, setPatient] = useState({
    name: "",
    dateOfBirth: null,
    isMale: true,
    phone: "",
    bloodGroupId: 0,
    address: "",
  });
  const [errors, setErrors] = useState({});
  const [alertConfig, setAlertConfig] = useState({
    isOpen: false,
    message: "",
    type: "info",
  });
  const [blood, setBlood] = useState([]);

  const getBlood = async () => {
    let result = await APIUtiliyWithJWT.get("/api/blood");
    if (result) {
      setBlood(result);
    }
  };
  const getPatient = async () => {
    if (id && !isNaN(id)) {
      let result = await APIUtiliyWithJWT.get(`/api/patients/${id}`);
      if (result) {
        setPatient({ ...result, dateOfBirth: formatDate(result.dateOfBirth) });
      }
    }
  };
  const addPatient = async () => {
    let result = await APIUtiliyWithJWT.post(`/api/patients`, patient);
    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const updatePatient = async () => {
    let result = await APIUtiliyWithJWT.put(`/api/patients/${id}`, {
      id: id,
      ...patient,
    });
    if (result) {
      showAlert("Data Updated Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  useEffect(() => {
    getBlood();
    getPatient();
  }, [id]);

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };
  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/patients");
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = {};
    if (!patient.name.trim()) {
      validationErrors.name = "Name is required";
    }

    if (!patient.dateOfBirth) {
      validationErrors.dateOfBirth = "Date of birth is required";
    }

    if (!patient.phone.trim()) {
      validationErrors.phone = "Phone is required";
    } else if (isNaN(patient.phone.trim())) {
      validationErrors.phone = "Phone is not valid";
    } else if (patient.phone.trim().length !== 10) {
      validationErrors.phone = "Phone should be 10 digits";
    }

    if (!patient.address.trim()) {
      validationErrors.address = "Address is required";
    }

    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      if (id && !isNaN(id)) {
        updatePatient();
      } else {
        addPatient();
      }
    }
  };

  const onChange = (e) => {
    let { name, value } = e.target;
    if (name === "isMale") {
      if (value === "true") {
        value = true;
      } else {
        value = false;
      }
    }
    setPatient({ ...patient, [name]: value });
  };

  return (
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
          {id && !isNaN(id) ? "Update" : "Add"} Patient
        </h4>

        <Input
          label="Name"
          name="name"
          value={patient.name}
          onChange={onChange}
          errorMessage={errors.name}
          required
        />
        <Input
          label="Date Of Birth"
          name="dateOfBirth"
          type="date"
          value={patient.dateOfBirth}
          onChange={onChange}
          errorMessage={errors.dateOfBirth}
          required
          min={`1950-01-01`}
          max={formatDate(
            new Date().setFullYear(new Date().getFullYear() - 18)
          )}
        />
        <Select
          label="Gender"
          name="isMale"
          value={patient.isMale}
          onChange={onChange}
          errorMessage={errors.isMale}
          options={[
            { value: true, label: "Male" },
            { value: false, label: "Female" },
          ]}
        />
        <Input
          label="Phonoe"
          name="phone"
          value={patient.phone}
          onChange={onChange}
          errorMessage={errors.phone}
          required
        />
        <Select
          label="Blood Group"
          name="bloodGroupId"
          value={patient.bloodGroupId}
          onChange={onChange}
          errorMessage={errors.bloodGroupId}
          options={
            blood &&
            blood.map((blood) => {
              return { value: blood.id, label: blood.bloodGroupName };
            })
          }
        />
        <Textarea
          label="Address"
          name="address"
          value={patient.address}
          onChange={onChange}
          errorMessage={errors.address}
          required
        />

        <Button
          style={{ marginBottom: "0.5rem" }}
          onClick={handleSubmit}
          className="btn-block"
          children={id && !isNaN(id) ? "Update" : "Add"}
        />
        <Link
          style={{ textAlign: "center" }}
          className="btn btn-block"
          to="/patients"
        >
          Cancel
        </Link>
        <Alert
          isOpen={alertConfig.isOpen}
          message={alertConfig.message}
          type={alertConfig.type}
          duration={alertConfig.duration}
          onClose={closeAlert}
        />
      </form>
    </section>
  );
};
export default AddUpdatePatient;
