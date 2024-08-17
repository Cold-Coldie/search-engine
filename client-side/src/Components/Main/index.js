import { Button, Card, Pagination, Stack, Typography } from "@mui/material";
import React, { useEffect, useState } from "react";
import MySearchTextField from "../Inputs/MySearchTextField";
import ButtonWithLoading from "../Buttons/ButtonWithLoading";
import FormToBuildNewInvertedIndex from "../Forms/FormToBuildNewInvertedIndex";
import PopupModal from "../PopupModal";
import axios from "axios";

const Main = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [searchText, setSearchText] = useState("");
  const [responseTime, setResponseTime] = useState(null); // Add state for response time
  const onSearchTextChange = (event) => {
    const value = event.target.value;
    setSearchText(value);
  };
  const [openSettingsPopup, setOpenSettingsPopup] = useState(false);
  const handleSettingsClick = () => {
    setOpenSettingsPopup(true);
  };
  const handleClosSettingsPopup = () => {
    setOpenSettingsPopup(false);
  };

  const [NumOfRelevantDocuments, setNumOfRelevantDocuments] = useState(null);
  const [data, setData] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const handlePageChange = (event, value) => {
    setPageNumber(value);
    onSubmit();
  };
  const [numOfPages, setNumOfPages] = useState(1);

  useEffect(() => {
    onSubmit();
  }, [pageNumber]);

  const onSubmit = async (e) => {
    if (searchText === "") {
      alert("Type a text to search.");
      return;
    }

    if (searchText !== "") {
      setIsLoading(true);
      const startTime = performance.now(); // Record start time

      await axios
        .get(`http://localhost:5090/api/documents`, {
          params: {
            searchText: searchText,
            pageSize: 10,
            pageNumber: pageNumber,
          },
          headers: {
            Accept: "application/json",
            "Content-Type": "multipart/form-data",
          },
        })
        .then((response) => {
          const endTime = performance.now(); // Record end time
          const timeTaken = ((endTime - startTime) / 1000).toFixed(2); // Calculate response time in seconds
          setResponseTime(timeTaken); // Set response time

          setData(response.data.documents);
          setNumOfPages(response.data.numOfPages);
          setNumOfRelevantDocuments(response.data.relevantDocumentsCount);
        })
        .catch((error) => {
          console.log(error);
          if (error.response) {
            var errorMessage = error.response.data.error;
            alert("Error: ", errorMessage);
          } else {
            alert("Error: ", error.message);
          }
        });
    }
    setIsLoading(false);
  };

  return (
    <Stack alignItems={"center"} spacing={3} paddingTop={10}>
      <Button
        sx={{
          position: "absolute",
          top: "15px",
          right: "15px",
          border: "1px solid",
        }}
        onClick={handleSettingsClick}
      >
        UPLOAD CSV
      </Button>
      <PopupModal
        name={"Build new inverted index"}
        open={openSettingsPopup}
        fullWidth={true}
        handleClose={handleClosSettingsPopup}
      >
        <FormToBuildNewInvertedIndex
          handleClosSettingsPopup={handleClosSettingsPopup}
        />
      </PopupModal>

      <div
        style={{
          color: "rgb(60, 64, 183)",
          fontSize: "3rem",
          margin: "3rem 0",
        }}
      >
        GROUP 3 SEARCH ENGINE
      </div>

      <MySearchTextField
        value={searchText}
        onChange={onSearchTextChange}
        placeholder={"type a search query"}
      />
      <Stack>
        <ButtonWithLoading
          label={"Search"}
          isLoading={isLoading}
          onClick={onSubmit}
        />
      </Stack>
      {data && (
        <Typography>
          Number of relevant Documents: {NumOfRelevantDocuments}
        </Typography>
      )}
      {responseTime && (
        <Typography>Response Time: {responseTime} seconds</Typography> // Display the response time in seconds
      )}
      {data && (
        <Pagination
          count={numOfPages}
          page={pageNumber}
          onChange={handlePageChange}
          color="primary"
        />
      )}
      {data && (
        <Stack
          width={"60%"}
          spacing={3}
          sx={{
            paddingBottom: "3rem",
          }}
        >
          {data.map((item) => (
            <Card
              key={item.id} // Make sure each card has a unique key
              sx={{
                padding: 3,
                boxShadow: "0 4px 20px rgba(0, 0, 0, 0.1)",
                borderRadius: "20px",
                background: "linear-gradient(135deg, #1e3a8a, #4f46e5)", // Blue gradient
                color: "#fff",
                transition: "transform 0.3s ease, box-shadow 0.3s ease",
                "&:hover": {
                  transform: "translateY(-10px)",
                  boxShadow: "0 8px 30px rgba(0, 0, 0, 0.2)",
                },
              }}
            >
              <Stack spacing={2}>
                <Typography
                  sx={{
                    backgroundColor: "rgba(255, 255, 255, 0.2)",
                    padding: 1,
                    borderRadius: "10px",
                    textAlign: "center",
                    fontWeight: "bold",
                    textShadow: "1px 1px 2px rgba(0, 0, 0, 0.2)",
                  }}
                >
                  Score: {item.score}
                </Typography>
                <Typography
                  sx={{
                    textShadow: "1px 1px 2px rgba(0, 0, 0, 0.2)",
                    lineHeight: 1.5,
                  }}
                >
                  {item.text}
                </Typography>
              </Stack>
            </Card>
          ))}
        </Stack>
      )}
    </Stack>
  );
};

export default Main;
