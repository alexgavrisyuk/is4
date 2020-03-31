import api  from "./axios";

export const post = (url) => {
  return new Promise((resolve, reject) => {
    api.post(`http://localhost:5000/api/${url}`)
    .then(res => {
      resolve(res);
    })
    .catch(err => {
      reject(err);
    })
  })
}
