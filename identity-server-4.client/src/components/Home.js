import React, { Component } from 'react';
import { Formik } from 'formik';
import Axios from 'axios';
import { connect } from 'react-redux';

class Home extends Component {
  componentDidMount = () => {
    console.log(this.props.user);
  }

  render(){
    return (
    <div>
      <Formik
      initialValues={{  }}
        validate={values => {
        }}
        onSubmit={(values, { setSubmitting }) => {

            Axios.get("http://localhost:5003/Account", {
              headers:{
                'Authorization': `Bearer ${this.props.user.access_token}`
              }
            }).then(res => {
              console.log(res);

            })
            .catch(err => {
              console.log(err);
            });


           

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
          
          <button type="submit" disabled={isSubmitting}>
            Submit
          </button>
        </form>
      )}
    </Formik>
    </div>
    )
  }
}

const mapStateToProps = state => {
  return {
    user: state.oidc.user
  }
}

const mapDispatchToProps = dispatch => {
  return {
  }
}
 export default connect(mapStateToProps, mapDispatchToProps)(Home);

// export default Home;