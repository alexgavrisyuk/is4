import React, { Component } from 'react';
import { useField, Formik } from 'formik';
import Axios from 'axios';
import userManager from '../utils/userManager';

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

class Login extends Component {
  render(){
    return(
    <>
      Login page

      <Formik
      initialValues={{ userName: '',  password:'' }}
        validate={values => {
        }}
        onSubmit={(values, { setSubmitting }) => {



           userManager.signinRedirect();

          // userManager.getUser()
          // .then(user => {
          //   console.log(user);
          // })
          // .catch(err => {
          //   console.log(err)
          // });

          // Axios.post(`http://localhost:5000/api/User/Login`, {
          //   ...values
          // })
          //       .then(res => {
          //         console.log(res);
          //       })
          //       .catch(err => {
          //         console.log(err);
          //       });
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
            label="userName"
            type="text"
            name="userName"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.firstName}
          />
          {errors.userName && touched.userName && errors.userName}
          <MyTextInput
            label="password"
            type="password"
            name="password"
            onChange={handleChange}
            onBlur={handleBlur}
            value={values.lastName}
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

export default Login;