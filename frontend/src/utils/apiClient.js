export class APIUtiliyWithJWT {
  static #getHeaders() {
    const accessToken = localStorage.getItem("accessToken");
    const headers = { "Content-Type": "application/json" };
    if (accessToken) {
      headers.Authorization = `Bearer ${accessToken}`;
    }
    return headers;
  }
  static async #refreshToken() {
    const refreshToken = localStorage.getItem("refreshToken");
    return fetch("/api/auth/refresh", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ refreshToken }),
    });
  }
  static async #fetchWithToken(url, options = {}) {
    const response = await fetch(url, {
      ...options,
      headers: {
        ...APIUtiliyWithJWT.#getHeaders(),
        ...options.headers,
      },
    });

    // Check for 401 to refresh token
    if (response.status === 401 && !options._retry) {
      const refreshResponse = await APIUtiliyWithJWT.#refreshToken();
      if (refreshResponse.ok) {
        const data = await refreshResponse.json();
        localStorage.setItem("accessToken", data.accessToken);

        // Retry the original request
        return APIUtiliyWithJWT.#fetchWithToken(url, {
          ...options,
          _retry: true,
        });
      } else {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        window.location.href = "/login";
        throw new Error("Unauthorized");
      }
    }

    return response;
  }
  // Get
  static async get(url) {
    let response = await APIUtiliyWithJWT.#fetchWithToken(url, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });
    const result = await response.json();
    return response.ok ? result : null;
  }
  // Put
  static async put(url, object) {
    let response = await APIUtiliyWithJWT.#fetchWithToken(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(object),
    });
    return response.ok ? true : false;
  }
  // Post
  static async post(url, object) {
    let response = await APIUtiliyWithJWT.#fetchWithToken(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(object),
    });
    return response.ok ? true : false;
  }
  // Delete
  static async delete(url) {
    let response = await APIUtiliyWithJWT.#fetchWithToken(url, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.ok ? true : false;
  }
}
