import { useEffect, useState } from "react";
import Input from "../../components/Input";
import { formatDate } from "../../utils/utils";
import Select from "../../components/Select";
import Alert from "../../components/Alert";
import Textarea from "../../components/Textarea";
import { APIUtiliyWithJWT } from "../../utils/apiClient";
import Button from "../../components/Button";
import { Link, useNavigate, useParams } from "react-router-dom";

const AddUpdateDonor = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [donor, setDonor] = useState({
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
  const getDonor = async () => {
    if (id && !isNaN(id)) {
      let result = await APIUtiliyWithJWT.get(`/api/donors/${id}`);
      if (result) {
        setDonor({ ...result, dateOfBirth: formatDate(result.dateOfBirth) });
      }
    }
  };
  const addDonor = async () => {
    let result = await APIUtiliyWithJWT.post(`/api/donors`, donor);
    if (result) {
      showAlert("Data Added Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  const updateDonor = async () => {
    let result = await APIUtiliyWithJWT.put(`/api/donors/${id}`, {
      id: id,
      ...donor,
    });
    if (result) {
      showAlert("Data Updated Successfuly", "success");
    } else {
      showAlert("Something Went Wrong", "error");
    }
  };

  useEffect(() => {
    getBlood();
    getDonor();
  }, [id]);

  const showAlert = (message, type = "info", duration) => {
    setAlertConfig({ isOpen: true, message, type, duration });
  };
  const closeAlert = () => {
    setAlertConfig((prev) => ({ ...prev, isOpen: false }));
    navigate("/donors");
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = {};
    if (!donor.name.trim()) {
      validationErrors.name = "Name is required";
    }

    if (!donor.dateOfBirth) {
      validationErrors.dateOfBirth = "Date of birth is required";
    }

    if (!donor.phone.trim()) {
      validationErrors.phone = "Phone is required";
    } else if (isNaN(donor.phone.trim())) {
      validationErrors.phone = "Phone is not valid";
    } else if (donor.phone.trim().length !== 10) {
      validationErrors.phone = "Phone should be 10 digits";
    }

    if (!donor.address.trim()) {
      validationErrors.address = "Address is required";
    }

    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      if (id && !isNaN(id)) {
        updateDonor();
      } else {
        addDonor();
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
    setDonor({ ...donor, [name]: value });
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
          {id && !isNaN(id) ? "Update" : "Add"} Donor
        </h4>

        <Input
          label="Name"
          name="name"
          value={donor.name}
          onChange={onChange}
          errorMessage={errors.name}
          required
        />
        <Input
          label="Date Of Birth"
          name="dateOfBirth"
          type="date"
          value={donor.dateOfBirth}
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
          value={donor.isMale}
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
          value={donor.phone}
          onChange={onChange}
          errorMessage={errors.phone}
          required
        />
        <Select
          label="Blood Group"
          name="bloodGroupId"
          value={donor.bloodGroupId}
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
          value={donor.address}
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
          to="/donors"
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
export default AddUpdateDonor;
