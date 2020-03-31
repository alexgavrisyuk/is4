import React from 'react';
import { toast, ToastContainer } from 'react-toastify';

function withHttpHandler (WrappedComponent, axios)  {
  return class extends React.Component {
    constructor(props) {
      super(props);

      this.state = { errorMessage: null, error: false };

      this.reqInterceptor = axios.interceptors.request.use(req => {
        // req.headers.Authorization = `Bearer ${this.props.user.access_token}`;

        this.setState({ error: null });
        return req;
      });
    }

    componentDidMount() {
      this.resInterceptor = axios.interceptors.response.use(res => {
      console.log('ок');
      if (res.data && res.data.hasOwnProperty('isSuccess') && !res.data.isSuccess) {
          this.setState({ errorMessage: res.data.errorMessage, error: true });
          return res;
        }
        return res;
      }, error => {
        console.log('буль');
        if (!error || !error.response) {
          this.setState({ errorMessage: 'Something went wrong', error: true });
          return;
        }

        switch (+error.response.status) {
          case 400:
            this.setState({ errorMessage: error.response.data.message ? error.response.data.message : 'Bad Request', error: true });
            break;
          case 401:
            this.setState({ errorMessage: 'Not Authorized', error: true });
            break;
          case 404:
            this.setState({ errorMessage: 'Not Found', error: true });
            break;
          case 504:
            this.setState({ errorMessage: 'Internal Server Error', error: true });
            break;
          case 500:
            this.setState({ errorMessage: error.response.data.message ? error.response.data.message : 'Something went wrong', error: true });
            break;
          default:
            this.setState({ errorMessage: 'Something went wrong', error: true });
            break;
        }

        throw error;
      });
    }

    componentDidUpdate(prevProps, prevState) {
      // console.log(prevProps);
      // console.log(prevState);
      // console.log(this.state.error);
      // if (this.state.error === true && prevState.error !== this.state.error) {
      //   this.notify();
      // }
    }

    componentWillUnmount() {
      console.log('asd');
      axios.interceptors.request.eject(this.reqInterceptor);
      axios.interceptors.response.eject(this.resInterceptor);
    }

    handleClose = () => this.setState({ error: false, errorMessage: null });

    notify = () => {
      toast.error(<>
        <a as='h3' >Error</a>
        <p>{this.state.errorMessage ? this.state.errorMessage : null}</p>
      </>, { autoClose: 10000, onClose: () => this.handleClose() });
    }

    render() {
      let wrappedCmp =
        <>
          <ToastContainer enableMultiContainer newestOnTop
           />
          <WrappedComponent  {...this.props} />
        </>;

      return wrappedCmp;
    }
  }

}

export default withHttpHandler;