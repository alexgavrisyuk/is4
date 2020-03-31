import React, { Component } from 'react';
import { Formik, useField } from 'formik';
import { post } from '../utils/httpClient';
import api from '../utils/axios';
import Axios from 'axios';

const MyTextInput = ({ label, ...props }) => {
  // useField() returns [formik.getFieldProps(), formik.getFieldMeta()]
  // which we can spread on <input> and also replace ErrorMessage entirely.
  const [field, meta] = useField(props);
  return (
    <>
      <label htmlFor={props.id || props.name}>{label}</label>
      <input className="text-input" {...field} {...props} />
      {meta.touched && meta.error ? (
        <div className="error">{meta.error}</div>
      ) : null}
    </>
  );
};

class Register extends Component {
  render(){
    return (
    <>
    Register
    <Formik
      initialValues={{ firstName: '', lastName: '', userName: '', phoneNumber: '+380', password:'', email:'' }}
        validate={values => {
        }}
        onSubmit={(values, { setSubmitting }) => {

          Axios.post(`http://localhost:5000/api/User/Register`, {
            ...values
          })
                .then(res => {
                  console.log(res);
                })
                .catch(err => {
                  console.log(err);
                });
            // post("User/Register")
            // .then(res => {
            //   console.log(res);
            // })
            // .catch(err => {
            //   console.log(err);
            setSubmitting(false);
        }}
      >
      {({
        values,
        errors,
        touched,
        handleChange,
        handleBlur,
        handleSubmit,
        isSubmitting

      }) => (
        <form onSubmit={handleSubmit}>
          <MyTextInput
            label="firstname"
            type="text"
            name="firstName"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.firstName}
          />
          {errors.firstName && touched.firstName && errors.firstName}
          <MyTextInput
            label="lastName"
            type="text"
            name="lastName"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.lastName}
          />
          {errors.lastName && touched.lastName && errors.lastName}
          <MyTextInput
            label="userName"
            type="phone"
            name="userName"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.userName}
          />
          {errors.userName && touched.userName && errors.userName}
          <MyTextInput
            label="phoneNumber"
            type="phoneNumber"
            name="phoneNumber"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.phoneNumber}
          />
          {errors.phoneNumber && touched.phoneNumber && errors.phoneNumber}
          <MyTextInput
            label="email"
            type="email"
            name="email"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.email}
          />
          {errors.email && touched.email && errors.email}
          <MyTextInput
            label="password"
            type="password"
            name="password"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.password}
          />
          {errors.password && touched.password && errors.password}
          <button type="submit" disabled={isSubmitting}>
            Submit
          </button>
        </form>
      )}
    </Formik>
    </>
    )
  }
}

export default Register;