import axios from 'axios';
import React, { useEffect, useState } from 'react';

const Lucturer = () => {
    const [lucturer, setLucturer] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [searchTerm, setSearchTerm] = useState("");
    const [searchInput, setSearchInput] = useState("");
    const [totalPages, setTotalPages] = useState(0);


    const handelSearch = (e) => {
        e.preventDefault();
        setSearchTerm(searchInput);
        setPageNumber(1);
    }

    const fetchLucturers = async () => {
        try {
            const res = await axios.get("https://localhost:7293/api/Lecturers", {
                params: {
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    search: searchTerm,
                }
            });
            setLucturer(res.data.data);
            setTotalPages(res.data.totalPage);
            console.log(res.data);
        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        fetchLucturers();
    }, [searchTerm, pageNumber, pageSize]);
    return (
        <div>
            <form action="" onSubmit={handelSearch}>
                <input type="text" value={searchInput} onChange={(e) => setSearchInput(e.target.value)} />
                <button type='submit'>tìm kiếm </button>
            </form>
            <table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Degree</th>
                        <th>Department</th>
                    </tr>
                </thead>
                <tbody>
                    {lucturer.map((lucturer) => (
                        <tr key={lucturer.lecturerId}>
                            <td>{lucturer.fullName}</td>
                            <td>{lucturer.degree}</td>
                            <td>{lucturer.departmentName}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div>
                <button disabled={pageNumber === 1} onClick={() => setPageNumber(pageNumber - 1)}>
                    trước
                </button>
                {Array.from({ length: totalPages }, (_, index) => index + 1)
                    .filter(page => page >= pageNumber - 1 && page <= pageNumber + 1)
                    .map(page => (
                        <button
                            key={page}
                            disabled={page === pageNumber}
                            onClick={() => setPageNumber(page)}
                        >
                            {page}
                        </button>
                    ))}
                <button disabled={pageNumber === totalPages} onClick={() => setPageNumber(pageNumber + 1)}>
                    sau
                </button>
            </div>

        </div>
    );
};

export default Lucturer;