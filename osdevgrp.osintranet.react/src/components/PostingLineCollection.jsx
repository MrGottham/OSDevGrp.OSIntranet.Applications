import Table from 'react-bootstrap/Table'
import Stack from 'react-bootstrap/Stack';

function PostingLineCollection({ postingLineCollection }) {
    return (
        <Stack gap={3}>
            <p className='mb-1 fw-bold'>{postingLineCollection.latestPostingsHeader}</p>
            <Table className='p-0' responsive={true}>
                <thead>
                    <tr>
                        <th className='text-nowrap'>{postingLineCollection.postingDateHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingLineCollection.postingReferenceHeader}</th>
                        <th className='d-none d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingLineCollection.accountHeader}</th>
                        <th>{postingLineCollection.postingTextHeader}</th>
                        <th className='d-none d-sm-none d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingLineCollection.budgetAccountHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingLineCollection.debitHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingLineCollection.creditHeader}</th>
                        <th className='d-table-cell d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-none d-xxl-none text-end text-nowrap'>{postingLineCollection.postingValueHeader}</th>
                    </tr>
                </thead>
                <tbody>
                    {postingLineCollection.postingLines.map(getPostingLineContent)}
                </tbody>
            </Table>
        </Stack>
    );

    function getPostingLineContent(postingLineDisplayer) {
        return (
            <tr key={postingLineDisplayer.identification}>
                <td className='text-nowrap'>{postingLineDisplayer.postingDate}</td>
                <td className='d-none d-sm-none d-md-none d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingLineDisplayer.postingReference}</td>
                <td className='d-none d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingLineDisplayer.account}</td>
                <td>{postingLineDisplayer.postingText}</td>
                <td className='d-none d-sm-none d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingLineDisplayer.budgetAccount}</td>
                <td className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingLineDisplayer.debit}</td>
                <td className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingLineDisplayer.credit}</td>
                <td className='d-table-cell d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-none d-xxl-none text-end text-nowrap'>{postingLineDisplayer.postingValue}</td>
            </tr>
        );
    }
}

export default PostingLineCollection;