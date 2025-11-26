import { useForm } from "react-hook-form";
import api from "../api/apiClient";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

interface RegisterFormData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  DailyCalorieGoal: number;
}

export default function Register() {
  const navigate = useNavigate();
  const [serverError, setServerError] = useState("");

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterFormData>();

  const onSubmit = async (data: RegisterFormData) => {
    setServerError("");

    try {
      const res = await api.post("/User", data);

      if (res.status === 200 || res.status === 201) {
        navigate("/login");
      }
    } catch (err: any) {
      if (err.response?.status === 400) {
        setServerError("Email already exists or invalid data.");
      } else {
        setServerError("Server error — try again.");
      }
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: "auto" }}>
      <h2>Create Account</h2>

      <form onSubmit={handleSubmit(onSubmit)}>

        <input
          placeholder="First Name"
          {...register("firstName", { required: "First name is required" })}
        />
        {errors.firstName && <p style={{ color: "red" }}>{errors.firstName.message}</p>}

        <input
          placeholder="Last Name"
          {...register("lastName", { required: "Last name is required" })}
        />
        {errors.lastName && <p style={{ color: "red" }}>{errors.lastName.message}</p>}

        <input
          type="email"
          placeholder="Email"
          {...register("email", {
            required: "Email is required",
            pattern: { value: /\S+@\S+\.\S+/, message: "Invalid email" },
          })}
        />
        {errors.email && <p style={{ color: "red" }}>{errors.email.message}</p>}

        <input
          type="password"
          placeholder="Password"
          {...register("password", {
            required: "Password is required",
            minLength: { value: 6, message: "Password must be 6+ characters" },
          })}
        />
        {errors.password && <p style={{ color: "red" }}>{errors.password.message}</p>}

        <input
          placeholder="Calorie Count"
          {...register("DailyCalorieGoal", { required: "Calorie Goal is needed" })}
        />
        {errors.DailyCalorieGoal && <p style={{ color: "red" }}>{errors.DailyCalorieGoal.message}</p>}

        <button type="submit" disabled={isSubmitting}>
          {isSubmitting ? "Creating..." : "Register"}
        </button>
      </form>

      {serverError && <p style={{ color: "red" }}>{serverError}</p>}
    </div>
  );
}
